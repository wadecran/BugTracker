using BugTracker.Models;
using BugTracker.Utilities;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{

    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ProjectHelper projectHelper = new ProjectHelper();
        TicketHelper ticketHelper = new TicketHelper();
        RoleHelper roleHelper = new RoleHelper();
        UserHelper userHelper = new UserHelper();

        public ActionResult Dashboard()
        {
            var projects = new List<Project>();
            var userProjects = new List<Project>();
            var tickets = new List<Ticket>();
            var userTickets = new List<Ticket>();
            var userView = new DashboardVM();

            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                var userId = User.Identity.GetUserId();
                projects = db.Projects.ToList();
                tickets = db.Tickets.ToList();
                userProjects = projectHelper.ListUserProjects(userId);
                userTickets = ticketHelper.ListUsersTickets(userId);
                userView = new DashboardVM()
                {
                    Projects = projects,
                    UserProjects = userProjects,
                    Tickets = tickets,
                    UserTickets = userTickets,
                    Managers = roleHelper.UsersInRole("ProjectManager").ToList(),
                    Developers = roleHelper.UsersInRole("Developer").ToList(),
                    Submitters = roleHelper.UsersInRole("Submitter").ToList(),
                    UnassignedProjects = projectHelper.ListUnassignedProjects(),
                    UnassignedTickets = ticketHelper.ListUnassignedTickets(),
                    UnassignedDevs = userHelper.ListUnassignedDevs(),
                    NoTicketDevs = userHelper.ListDevWithoutTicket(),
                    UnassignedSubs = userHelper.ListUnassignedSubmitters()
                };
            }
            else
            {
                var userId = User.Identity.GetUserId();
                userProjects = projectHelper.ListUserProjects(userId);
                userTickets = ticketHelper.ListUsersTickets(userId);
                userView = new DashboardVM() 
                { 
                    UserProjects = userProjects,
                    UserTickets = userTickets
                };
            }
            return View(userView);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _SideNav()
        {
            ICollection<Project> userProjects;
            ICollection<Ticket> userTickets;
            ICollection<ApplicationUser> userList;
            var userView = new _SideNavVM();

            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                userProjects = db.Projects.ToList();
                userTickets = db.Tickets.ToList();
                userList = db.Users.ToList();

                userView = new _SideNavVM()
                {
                    Projects = userProjects,
                    Tickets = userTickets,
                    Users = userList
                };
            }
            else
            {
                userView = new _SideNavVM()
                {
                    Projects = projectHelper.ListUserProjects(User.Identity.GetUserId()),
                    Tickets = ticketHelper.ListUsersTickets(User.Identity.GetUserId())
                };
            }

            return PartialView(userView);
        }
    }
}