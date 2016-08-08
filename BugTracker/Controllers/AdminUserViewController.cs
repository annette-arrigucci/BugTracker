using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class AdminUserViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminUserView
        public ActionResult Index()
        {
            UserRolesHelper helper = new UserRolesHelper(db);
            var userRoles = new HashSet<AdminUserViewModel>();
            foreach(ApplicationUser user in db.Users)
            {
                var myUser = new AdminUserViewModel();
                myUser.UserId = user.Id;
                myUser.UserEmail = user.Email;
                myUser.RolesList = helper.ListUserRoles(user.Id).ToString();
                userRoles.Add(myUser);
            }
            return View(userRoles);
        }
        //get method
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            var user = db.Users.Find(id);
            AdminUserViewModel AdminModel = new AdminUserViewModel();
            UserRolesHelper helper = new UserRolesHelper(db);
            var selected = helper.ListUserRoles(id);
            AdminModel.Roles = new MultiSelectList(db.Roles, "Name", "Name", selected);
            AdminModel.UserId = user.Id;

            return View(AdminModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "UserId,SelectedRoles")] AdminUserViewModel admModel)
        {
            var user = db.Users.Find(admModel.UserId);
            var id = admModel.UserId;
            UserRolesHelper helper = new UserRolesHelper(db);
            var allRoles = new HashSet<string>();

            foreach (var role in db.Roles)
            {
                var myRole = role.ToString();
                allRoles.Add(myRole);
            }

                if (admModel.SelectedRoles != null)
                {
                    var rolesToRemove = allRoles.Except(admModel.SelectedRoles);
                //foreach(var role in db.Roles)
                //{
                //    if (helper.IsUserInRole(id, myRole))
                //    {

                //    }
                    //var myRole = role.ToString();
                    //if(!helper.IsUserInRole(id, myRole))
                    //{
                    //    helper.AddUserToRole(id, myRole);
                    //}
                }
            }

            return View(AdminModel);
        }


    }
}