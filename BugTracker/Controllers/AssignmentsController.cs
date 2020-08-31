using BugTracker.Models;
using BugTracker.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [RequireHttps]
    public class AssignmentsController : Controller

    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper rolesHelper = new RoleHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        private TicketHelper ticketHelper = new TicketHelper();

        #region Role Assignments
        // GET: Assignments
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(string userId, string roleName)
        {
                foreach (var role in rolesHelper.ListUserRoles(userId).ToList())
                {
                    rolesHelper.RemoveUserFromRole(userId, role);
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    rolesHelper.AddUserToRole(userId, roleName);
                }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ManageUserRoles()
        {
            return View();
        }
        #endregion

        #region Project Assignments

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(string userId, List<int> addProjectIds, List<int> removeProjectIds)
        {
            if (addProjectIds != null)
            {
                foreach (var projectId in addProjectIds)
                {
                    projHelper.AddUserToProject(userId, projectId);
                }
            }
            if (removeProjectIds != null)
            {
                foreach (var projectId in removeProjectIds)
                {
                    projHelper.RemoveUserFromProject(userId, projectId);
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectDevs(List<string> devIds, int projectId)
        {

            foreach (var userId in devIds)
            {
                projHelper.AddUserToProject(userId, projectId);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectSubs(List<string> subIds, int projectId)
        {

            foreach (var userId in subIds)
            {
                projHelper.AddUserToProject(userId, projectId);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectManager(string managerId, int projectId)
        {
            var oldManager = projHelper.ListProjectUsersInRole(projectId, "ProjectManager");

            foreach (var manager in oldManager)
            {
                    projHelper.RemoveUserFromProject(manager.Id, projectId);
            }

            projHelper.AddUserToProject(managerId, projectId);
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region Ticket Assignments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserTickets(string userId, List<int> addTicketIds, List<int> removeTicketIds)
        {
            if (addTicketIds != null)
            {
                foreach (var ticketId in addTicketIds)
                {
                    ticketHelper.AssignTicket(userId, ticketId);
                }
            }

            if (removeTicketIds != null)
            {
                foreach (var ticketId in removeTicketIds)
                {
                    ticketHelper.UnAssignTicket(ticketId);
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageTicketUser(string userId, int ticketId)
        {
            if (userId != null)
            {
                ticketHelper.AssignTicket(userId, ticketId);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion
    }
}