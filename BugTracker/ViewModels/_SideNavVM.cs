using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace BugTracker.ViewModels
{
    public class _SideNavVM
    {
        public ICollection<Project> Projects { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public _SideNavVM()
        {
            Projects = new HashSet<Project>();
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}