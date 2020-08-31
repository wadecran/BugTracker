using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class ProjectEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }

        public bool IsArchived { get; set; }
    }
}