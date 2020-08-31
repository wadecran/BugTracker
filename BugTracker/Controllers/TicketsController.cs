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
    //[Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelp = new ProjectHelper();
        private RoleHelper roleHelp = new RoleHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private NotificationHelper noteHelper = new NotificationHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        // GET: Tickets
        public ActionResult Index()
        {
            TicketIndexVM userView = new TicketIndexVM();
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                userView = new TicketIndexVM()
                {
                    Tickets = db.Tickets.ToList(),
                    UserTickets = ticketHelper.ListUsersTickets(User.Identity.GetUserId())
                };
            }
            else
            {
                userView = new TicketIndexVM()
                {
                    UserTickets = ticketHelper.ListUsersTickets(User.Identity.GetUserId())
                };
            }

            return View(userView);
        }
        public ActionResult GetProjectTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();
            return View("Index", ticketList);
        }

        public ActionResult GetMyTickets()
        {
            var userId = User.Identity.GetUserId();
            var ticketList = new List<Ticket>();
            if (User.IsInRole("Developer"))
            {
                ticketList = db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                return View("Index", ticketList);
            }
            if (User.IsInRole("Submitter"))
            {
                ticketList = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                return View("Index", ticketList);
            }
            else
            {
                return RedirectToAction("GetProjectTickets");
            }

        }
        // GET: Tickets/Details/5
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var projectDevs = projHelp.ListProjectUsersInRole(ticket.ProjectId, "Developer");
            ViewBag.NotAssignedDev = new SelectList(projectDevs.Where(u => u.Id != ticket.DeveloperId));

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(projHelp.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketTypeId,Issue,IssueDescription")] Ticket ticket)
        {
            if (ticket.Issue == null)
            {
                TempData["TicketErrors"] += "<p class=\"text-danger\">You must define an Issue</p>";
            }
            if (ticket.IssueDescription == null)
            {
                TempData["TicketErrors"] += "<p class=\"text-danger\">You must define a Description</p>";
            }

            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                
                    if (db.Projects.Find(ticket.ProjectId).Tickets.Any(t => t.Issue == ticket.Issue))
                    {
                        TempData["TicketErrors"] += "<p  class=\"text-danger\">A Ticket with this issue already exists</p>";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
          
                ticket.TicketStatusId = db.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;
                ticket.Created = DateTime.Now;
                ticket.SubmitterId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                TempData["TicketErrors"] += "Success";
                return Redirect(Request.UrlReferrer.ToString());
            }

            ViewBag.ProjectId = new SelectList(projHelp.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.DeveloperId = new SelectList(roleHelp.UsersInRole("Developer"), "Id", "Email");
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TicketEditVM ticket, int ticketId)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
                var thisTicket = db.Tickets.Find(ticketId);

                thisTicket.TicketPriorityId = ticket.TicketPriorityId;
                thisTicket.TicketTypeId = ticket.TicketTypeId;
                thisTicket.IssueDescription = ticket.IssueDescription;
                thisTicket.Issue = ticket.Issue;

                if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
                {
                    thisTicket.DeveloperId = ticket.DeveloperId;
                    thisTicket.IsResolved = ticket.IsResolved;
                    thisTicket.IsArchived = ticket.IsArchived;
                    thisTicket.TicketStatusId = ticket.TicketStatusId;
                }


                thisTicket.Updated = DateTime.Now;

                db.SaveChanges();

                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

                noteHelper.ManageTicketNotifications(oldTicket, newTicket);
                historyHelper.ManageHistories(oldTicket, newTicket);

                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.DeveloperId = new SelectList(roleHelp.UsersInRole("Developer"), "Id", "Email");
            return Redirect(Request.UrlReferrer.ToString());
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
