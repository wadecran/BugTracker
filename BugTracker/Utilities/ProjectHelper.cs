using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Utilities
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();

        public bool IsUserOnProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            return project.Users.Any(u => u.Id == userId);
        }
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project project = db.Projects.Find(projectId);
                ApplicationUser user = db.Users.Find(userId);
                project.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void RemoveUseFromoProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            ApplicationUser user = db.Users.Find(userId);
            project.Users.Remove(user);
            db.SaveChanges();
        }

        public List<ApplicationUser> ProjectUsers(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var resultList = new List<ApplicationUser>();
            resultList.AddRange(project.Users);
            return resultList;
        }
        public bool IsProjectUser(int projectId, string userId)
        {
            Project project = db.Projects.Find(projectId);
            ApplicationUser user = db.Users.Find(userId);
            return project.Users.Contains(user);

        }
        public List<ApplicationUser> NotProjectUsers(int projectId)
        {
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if (!IsProjectUser(projectId, user.Id))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        //List users on a project that occupy a specific role
        public List<ApplicationUser> ListProjectUsersInRole(int projectId, string roleName)
        {
            var userList = ProjectUsers(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        //List users not on a project in a specific role
        public List<ApplicationUser> ListNotProjectUsersInRole(int projectId, string roleName)
        {
            var userList = NotProjectUsers(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }


        public List<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            var resultList = new List<Project>();
            resultList.AddRange(user.Projects);
            return resultList;
        }


    }
}