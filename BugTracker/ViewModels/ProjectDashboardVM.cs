using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class ProjectDashboardVM
    {
        public Project Project { get; set; }

        public ICollection<Ticket> UnassignedTickets { get; set; }

        public ApplicationUser ProjectManager { get; set; }

        public ICollection<ApplicationUser> ProjectDevs { get; set; }

        public ICollection<ApplicationUser> ProjectSubs { get; set; }

        public ICollection<ApplicationUser> NotProjectUsers { get; set; }

        public ProjectDashboardVM()
        {
            UnassignedTickets = new HashSet<Ticket>();
            ProjectDevs = new HashSet<ApplicationUser>();
            ProjectSubs = new HashSet<ApplicationUser>();
            NotProjectUsers = new HashSet<ApplicationUser>();
        }
    }
}