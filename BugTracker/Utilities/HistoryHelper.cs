using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Utilities
{
    public class HistoryHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void ManageHistories(Ticket oldTicket, Ticket newTicket)
        {
            DeveloperUpdate(oldTicket, newTicket);
            PriorityUpdate(oldTicket, newTicket);
            StatusUpdate(oldTicket, newTicket);
            TypeUpdate(oldTicket, newTicket);
            IssueUpdate(oldTicket, newTicket);
            DescriptionUpdate(oldTicket, newTicket);
            db.SaveChanges();
        }

        private void DeveloperUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if(oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    Property = "Developer",
                    OldValue = oldTicket.Developer == null ? "No Developer Assigned" : oldTicket.Developer.FullName,
                    NewValue = newTicket.DeveloperId == null ? "No Developer Assigned" : newTicket.Developer.FullName,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
        }

        private void PriorityUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    Property = "Priority",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
        }

        private void StatusUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    Property = "Status",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
        }

        private void TypeUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    Property = "Type",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
        }

       private void IssueUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.Issue != newTicket.Issue)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    Property = "Issue",
                    OldValue = oldTicket.Issue,
                    NewValue = newTicket.Issue,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
        }

        private void DescriptionUpdate(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.IssueDescription != newTicket.IssueDescription)
            {
                var history = new TicketHistory()
                {
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    Property = "Description",
                    OldValue = oldTicket.IssueDescription,
                    NewValue = newTicket.IssueDescription,
                    ChangedOn = DateTime.Now
                };
                db.TicketHistories.Add(history);
            }
        }
    }
}