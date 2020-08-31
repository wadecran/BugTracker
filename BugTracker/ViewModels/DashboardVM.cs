using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace BugTracker.ViewModels
{
    public class DashboardVM
    {
        public ICollection<Project> Projects { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Project> UserProjects { get; set; }

        public ICollection<Ticket> UserTickets { get; set; }

        public ICollection<ApplicationUser> Managers { get; set; }

        public ICollection<ApplicationUser> Developers { get; set; }

        public ICollection<ApplicationUser> Submitters { get; set; }

        public ICollection<Project> UnassignedProjects { get; set; }

        public ICollection<Ticket> UnassignedTickets { get; set; }

        public ICollection<ApplicationUser> UnassignedDevs { get; set; }

        public ICollection<ApplicationUser> NoTicketDevs { get; set; }

        public ICollection<ApplicationUser> UnassignedSubs { get; set; }


        public DashboardVM()
        {
            Projects = new HashSet<Project>();
            Tickets = new HashSet<Ticket>();
            Managers = new HashSet<ApplicationUser>();
            Developers = new HashSet<ApplicationUser>();
            Submitters = new HashSet<ApplicationUser>();
            UnassignedProjects = new HashSet<Project>();
            UnassignedTickets = new HashSet<Ticket>();
            UnassignedDevs = new HashSet<ApplicationUser>();
            NoTicketDevs = new HashSet<ApplicationUser>();
            UnassignedSubs = new HashSet<ApplicationUser>();
        }
    }
}