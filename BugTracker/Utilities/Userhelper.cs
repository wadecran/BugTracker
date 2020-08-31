using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Utilities
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        public string GetUserEmail(string userId)
        {
            return db.Users.Find(userId).UserName;
        }

        public string GetFullName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FullName;
        }

        public string LastNameFirst(string userId) 
        {
            var user = db.Users.Find(userId); ;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return lastName + " " + firstName ;
            
        }

        public List<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            var resultList = new List<Project>();
            resultList.AddRange(user.Projects);
            return resultList;
        }

        public string GetAvatarPath(string userId)
        {
            var user = db.Users.Find(userId);
            return user.AvatarPath;
        }

        public List<ApplicationUser> ListUnassignedDevs()
        {
            var resultList = new List<ApplicationUser>();
            foreach(var Dev in roleHelper.UsersInRole("Developer"))
            {
                if(Dev.Projects == null)
                {
                    resultList.Add(Dev);
                }
            }

            return resultList;
        }

        public List<ApplicationUser> ListDevWithoutTicket()
        {
            var resultList = new List<ApplicationUser>();
            foreach (var Dev in roleHelper.UsersInRole("Developer"))
            {
                if (!db.Tickets.Any(t => t.DeveloperId == Dev.Id))
                {
                    resultList.Add(Dev);
                }

            }

            return resultList;
        }

        public List<ApplicationUser> ListUnassignedSubmitters()
        {
            var resultList = new List<ApplicationUser>();
            foreach(var Sub in roleHelper.UsersInRole("Submitter"))
            {
                if(Sub.Projects == null)
                {
                    resultList.Add(Sub);
                }
            }

            return resultList;
        }
    }
}