using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class ProjectIndexVM
    {
        public ICollection<Project> Projects { get; set; }

        public ICollection<Project> UserProjects { get; set; }

        public ProjectIndexVM()
        {
            Projects = new HashSet<Project>();
            UserProjects = new HashSet<Project>();
        }
    }
}