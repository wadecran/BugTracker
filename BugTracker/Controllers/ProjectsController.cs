using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Utilities;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelp = new ProjectHelper();
        private RoleHelper roleHelp = new RoleHelper();
        private TicketHelper ticketHelp = new TicketHelper();

        // GET: Projects
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                var userView = new ProjectIndexVM()
                {
                    Projects = db.Projects.ToList(),
                    UserProjects = projHelp.ListUserProjects(HttpContext.User.Identity.GetUserId())
                };
                return View(userView);
            }
            else
            {
                var userView = new ProjectIndexVM()
                {
                    UserProjects = projHelp.ListUserProjects(HttpContext.User.Identity.GetUserId())
                };
                return View(userView);
            }

        }

        // GET: Projects/Details/5
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thisProject = db.Projects.Find(id);

            if (thisProject == null)
            {
                return HttpNotFound();
            }

            var project = new ProjectDashboardVM()
            {
                Project = thisProject,
                ProjectManager = projHelp.ListProjectUsersInRole(thisProject.Id, "ProjectManager").FirstOrDefault(),
                ProjectDevs = projHelp.ListProjectUsersInRole(thisProject.Id, "Developer"),
                ProjectSubs = projHelp.ListProjectUsersInRole(thisProject.Id, "Submitter"),
                NotProjectUsers = projHelp.NotProjectUsers(thisProject.Id),
                UnassignedTickets = ticketHelp.ListUnassignedTickets(thisProject.Id)
            };

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View(project);
        }

        public ActionResult List()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Projects.ToList());
            }
            return View(projHelp.ListUserProjects(User.Identity.GetUserId()));
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create([Bind(Include = "Id,Name,Details,Created,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectWizard(ProjectWizardVM model)
        {
            #region Fail Cases
            if (model.Name == null)
            {
                TempData["ProjectErrors"] += "<p class=\"text-danger\">You must define a Project Name</p>";
            }
            if (model.Details == null)
            {
                TempData["ProjectErrors"] += "<p class=\"text-danger\">You must define Project Details</p>";
            }
            if (model.ProjectManagerId == null)
            {
                TempData["ProjectErrors"] += "<p class=\"text-danger\">You must select a Project Manager</p>";
            }
            if (model.DeveloperIds.Count == 0)
            {
                TempData["ProjectErrors"] += "<p  class=\"text-danger\">You must select at least one Developer</p>";
            }
            if (model.SubmitterIds.Count == 0)
            {
                TempData["ProjectErrors"] += "<p  class=\"text-danger\">You must select at least one Submitter</p>";
            }
            #endregion
            if (ModelState.IsValid)
            {
                Project project = new Project();
                if(db.Projects.Any(p => p.Name == model.Name))
                {
                    TempData["ProjectErrors"] += "<p  class=\"text-danger\">A Project with this name already exists</p>";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                project.Name = model.Name;
                project.Details = model.Details;
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.Projects.Add(project);
                db.SaveChanges();
                TempData["ProjectErrors"] += "Success";

                projHelp.AddUserToProject(model.ProjectManagerId, project.Id);
                foreach (var userId in model.DeveloperIds)
                {
                    projHelp.AddUserToProject(userId, project.Id);
                }
                foreach (var userId in model.SubmitterIds)
                {
                    projHelp.AddUserToProject(userId, project.Id);
                }

                //return Redirect(returnUrl);
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectEditVM project, int projectId)
        {
            if (ModelState.IsValid)
            {
                var thisProject = db.Projects.Find(projectId);

                thisProject.Name = project.Name;
                thisProject.Details = project.Details;
                thisProject.IsArchived = project.IsArchived;

                db.SaveChanges();

                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
