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
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));

            //create Admin role
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            //create a Project Manager role
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            //create a Developer role
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            //create a Submitter role
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

           var userManager = new UserManager<ApplicationUser>(
           new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "bdavis@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "bdavis@coderfoundry.com",
                    Email = "bdavis@coderfoundry.com",
                    FirstName = "Bobby",
                    LastName = "Davis",
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "jtwichell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jtwichell@coderfoundry.com",
                    Email = "jtwichell@coderfoundry.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "annette.arrigucci@outlook.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "annette.arrigucci@outlook.com",
                    Email = "annette.arrigucci@outlook.com",
                    FirstName = "Annette",
                    LastName = "Arrigucci",
                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "annette.arrigucci@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "annette.arrigucci@gmail.com",
                    Email = "annette.arrigucci@gmail.com",
                    FirstName = "Christine",
                    LastName = "Arrigucci",
                }, "Abc&123!");
            }

            //set the users we created into the different roles
            var userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Admin");

            var userId2 = userManager.FindByEmail("jtwichell@coderfoundry.com").Id;
            userManager.AddToRole(userId2, "Project Manager");

            var userId3 = userManager.FindByEmail("annette.arrigucci@outlook.com").Id;
            userManager.AddToRole(userId3, "Developer");

            var userId4 = userManager.FindByEmail("annette.arrigucci@gmail.com").Id;
            userManager.AddToRole(userId4, "Submitter");
        }
    }
}
