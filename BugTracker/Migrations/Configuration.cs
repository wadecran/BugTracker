namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(
               new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "Admin@mail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    FirstName = "Ad",
                    LastName = "Min",
                    Email = "Admin@mail.com",
                    UserName = "Admin@mail.com",


                }, "OpenMe123");
                var userId = userManager.FindByEmail("Admin@mail.com").Id;
                userManager.AddToRoles(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "ProjectManager@mail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    FirstName = "Project",
                    LastName = "Manager",
                    Email = "ProjectManager@mail.com",
                    UserName = "ProjectManager@mail.com",


                }, "OpenMe123");
                var userId = userManager.FindByEmail("ProjectManager@mail.com").Id;
                userManager.AddToRoles(userId, "ProjectManager");
            }

            if (!context.Users.Any(u => u.Email == "Developer@mail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    FirstName = "Dev",
                    LastName = "Eloper",
                    Email = "Developer@mail.com",
                    UserName = "Developer@mail.com",


                }, "OpenMe123");
                var userId = userManager.FindByEmail("Developer@mail.com").Id;
                userManager.AddToRoles(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "Submitter@mail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    FirstName = "Sub",
                    LastName = "Mitter",
                    Email = "Submitter@mail.com",
                    UserName = "Submitter@mail.com",


                }, "OpenMe123");
                var userId = userManager.FindByEmail("Submitter@mail.com").Id;
                userManager.AddToRoles(userId, "Submitter");
            }
            context.SaveChanges();
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Other" }
                );

            #endregion

            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
                tt => tt.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );
            #endregion
            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
                tt => tt.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Archived" }
                );
            #endregion
            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed 1", Details = "Test", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Seed 2", Details = "Test", Created = DateTime.Now.AddDays(-30) },
                new Project() { Name = "Seed 3", Details = "Test", Created = DateTime.Now.AddDays(-15) },
                new Project() { Name = "Seed 4", Details = "Test", Created = DateTime.Now.AddDays(-10) },
                new Project() { Name = "Seed 5", Details = "Test", Created = DateTime.Now.AddDays(-7) }
                );
            #endregion
            context.SaveChanges();
        }
    }
}
