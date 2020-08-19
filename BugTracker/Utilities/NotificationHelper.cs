using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Utilities
{
    public class NotificationHelper
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public List<TicketNotification> ListUsersNotifications(string userId)
        {
            return db.TicketNotifications.Where(n => n.UserId == userId).OrderByDescending(n => n.Created).ToList();
        }

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
    }
}