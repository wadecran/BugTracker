using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Utilities
{
    public class Userhelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetUserEmail(string userId)
        {
            return db.Users.Find(userId).UserName;
        }

        public string GetFullName(string userId)
        {
            var user = db.Users.Find(userId); ;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }

        public string LastNameFirst(string userId) 
        {
            var user = db.Users.Find(userId);
            return user.FullName;
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

        //public ICollection<Project> ListUserProjects(string userId)
        //{
        //    var user = db.Users.Find(userId);
        //    return db.Projects.Where(p => p.Users.Contains(user)).Tolist
        //}
    }
}