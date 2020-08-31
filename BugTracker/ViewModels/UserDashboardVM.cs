using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;

namespace BugTracker.Utilities
{
    public class UserDashboardVM
    {
        public ApplicationUser User { get; set; }

        public ICollection<Project> NotUserProjects { get; set; }

        public ICollection<Ticket>  NotUserTickets { get; set; }

        public UserDashboardVM()
        {
            NotUserProjects = new HashSet<Project>();
            NotUserTickets = new HashSet<Ticket>();
        }
    }
}