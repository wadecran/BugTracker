using BugTracker.Models;
using BugTracker.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
        private UserRoleHelper rolesHelper = new UserRoleHelper();
        private ProjectHelper projHelper = new ProjectHelper();

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
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds == null)
                return RedirectToAction("RoleIndex");

            foreach (var userId in userIds)
            {
                foreach (var role in rolesHelper.ListUserRoles(userId).ToList())
                {
                    rolesHelper.RemoveUserFromRole(userId, role);
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    rolesHelper.AddUserToRole(userId, roleName);
                }
            }


            return RedirectToAction("ManageRoles");
        }

        public ActionResult ManageUserRoles()
        {
            return View();
        }
        #endregion

        #region Project Assignments
        public ActionResult ManageProjectUsers()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            if (User.IsInRole("Admin"))
            {
                ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");
            }
            else
            {
                ViewBag.ProjectIds = new MultiSelectList(projHelper.ListUserProjects(User.Identity.GetUserId()), "Id", "Name");
            }

            return View(db.Users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<string> userIds, List<int> projectIds)
        {
            //Case 1: No Users and no projects
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectUsers");
            }

            foreach (var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projHelper.AddUserToProject(userId, projectId);
                }
            }




            return RedirectToAction("ManageProjectUsers");
        }
        #endregion
    }
}