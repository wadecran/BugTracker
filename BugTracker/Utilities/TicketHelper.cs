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
        private UserRoleHelper roleHelp = new UserRoleHelper();
        private ApplicationDbContext db = new ApplicationDbContext();

        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                if (newTicket.DeveloperId != null)
                {
                    var newNotification = new TicketNotification()
                    {
                        TicketId = newTicket.Id,
                        UserId = newTicket.DeveloperId,
                        Created = DateTime.Now,
                        Subject = $"You have been assigned Ticket Id: {newTicket.Id}",
                        Body = $"Hello {newTicket.Developer.FullName}, you have been assigned to Ticket Id {newTicket.Id} titled '{newTicket.Issue}' on Project '{newTicket.Project}'"
                    };

                    db.TicketNotifications.Add(newNotification);
                    db.SaveChanges();
                }
                if (oldTicket.DeveloperId != null && newTicket.DeveloperId != null)
                {
                    var newAssignmentNotification = new TicketNotification()
                    {
                        TicketId = newTicket.Id,
                        UserId = newTicket.DeveloperId,
                        Created = DateTime.Now,
                        Subject = $"You have been assigned Ticket Id: {newTicket.Id}",
                        Body = $"Hello {newTicket.Developer.FullName}, you have been assigned to Ticket Id {newTicket.Id} titled '{newTicket.Issue}' on Project '{newTicket.Project}'"
                    };
                    db.TicketNotifications.Add(newAssignmentNotification);

                    var newUnassignmentNotification = new TicketNotification()
                    {
                        TicketId = oldTicket.Id,
                        UserId = oldTicket.DeveloperId,
                        Created = DateTime.Now,
                        Subject = $"You have been unassigned from Ticket Id: {newTicket.Id}",
                        Body = $"Hello {oldTicket.Developer.FullName}, you have been unassigned from Ticket Id {oldTicket.Id} titled '{oldTicket.Issue}' on Project '{oldTicket.Project}'"
                    };
                    db.TicketNotifications.Add(newUnassignmentNotification);
                    db.SaveChanges();
                }
                if (oldTicket.DeveloperId != null)
                {
                    var newUnassignmentNotification = new TicketNotification()
                    {
                        TicketId = oldTicket.Id,
                        UserId = oldTicket.DeveloperId,
                        Created = DateTime.Now,
                        Subject = $"You have been unassigned from Ticket Id: {newTicket.Id}",
                        Body = $"Hello {oldTicket.Developer.FullName}, you have been unassigned from Ticket Id {oldTicket.Id} titled '{oldTicket.Issue}' on Project '{oldTicket.Project}'"
                    };
                    db.TicketNotifications.Add(newUnassignmentNotification);
                    db.SaveChanges();
                }

            }
            //Unassignments

            //Reassignments
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
    }
}