using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Utilities
{
    public class TicketHelper
    {
        private RoleHelper roleHelp = new RoleHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private HistoryHelper historyHelper = new HistoryHelper();


        public List<Ticket> ListUsersTickets(string userId)
        {
            var resultList = new List<Ticket>();
            foreach (var ticket in db.Tickets.ToList())
            {
                switch (roleHelp.ListUserRoles(userId).FirstOrDefault())
                {
                    case "Admin":
                    case "ProjectManager":
                        if (db.Users.Find(userId).Projects.Contains(ticket.Project))
                        {
                            resultList.Add(ticket);
                        }

                        break;
                    default:
                        if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                        {
                            resultList.Add(ticket);
                        }
                        break;

                }
            }


            return resultList;

        }

        public List<Ticket> ListNotUserTickets(string userId)
        {
            var resultList = new List<Ticket>();
            foreach (var ticket in db.Tickets.ToList())
            {
                switch (roleHelp.ListUserRoles(userId).FirstOrDefault())
                {
                    case "Admin":
                    case "ProjectManager":
                        if (!db.Users.Find(userId).Projects.Contains(ticket.Project))
                        {
                            resultList.Add(ticket);
                        }

                        break;
                    default:
                        if (ticket.DeveloperId != userId || ticket.SubmitterId != userId)
                        {
                            resultList.Add(ticket);
                        }
                        break;

                }
            }
            return resultList;
        }
        public bool CanEditTicket(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelp.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                case "Submitter":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }
        public bool CanMakeComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelp.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                case "Submitter":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }
        public bool CanViewDetails(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelp.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                case "Submitter":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public bool CanAddAttachment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelp.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    return true;
                case "ProjectManager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                case "Submitter":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public List<Ticket> ListUnassignedTickets()
        {
            var resultList = new List<Ticket>();

            foreach (var ticket in db.Tickets.ToList())
            {
                if (ticket.DeveloperId == null)
                {
                    resultList.Add(ticket);
                }
            }

            return resultList;
        }

        public List<Ticket> ListUnassignedTickets(int id)
        {
            var resultList = new List<Ticket>();
            Project project = db.Projects.Find(id);
            foreach (var ticket in project.Tickets)
            {
                if (ticket.DeveloperId == null)
                {
                    resultList.Add(ticket);
                }
            }

            return resultList;
        }

        public List<TicketPriority> GetTicketPriorities()
        {
            var resultList = new List<TicketPriority>();

            foreach (var priority in db.TicketPriorities)
            {
                resultList.Add(priority);
            }

            return resultList;
        }

        public List<TicketStatus> GetTicketStatuses()
        {
            var resultList = new List<TicketStatus>();

            foreach (var status in db.TicketStatuses)
            {
                resultList.Add(status);
            }

            return resultList;
        }
        public List<TicketType> GetTicketTypes()
        {
            var resultList = new List<TicketType>();

            foreach (var type in db.TicketTypes)
            {
                resultList.Add(type);
            }

            return resultList;
        }

        public void AssignTicket(string userId, int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

            db.Tickets.Find(ticketId).DeveloperId = userId;
            db.SaveChanges();

            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            notificationHelper.ManageTicketNotifications(oldTicket, newTicket);
            historyHelper.ManageHistories(oldTicket, newTicket);
        }

        public void UnAssignTicket(int ticketId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

            db.Tickets.Find(ticketId).DeveloperId = null;
            db.SaveChanges();

            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);
            notificationHelper.ManageTicketNotifications(oldTicket, newTicket);
            historyHelper.ManageHistories(oldTicket, newTicket);
        }
    }
}