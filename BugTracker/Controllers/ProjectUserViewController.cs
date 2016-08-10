﻿using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ProjectUserViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectUserView
        [Authorize(Roles ="Admin, Project Manager")]
        public ActionResult Edit(int? projectId)
        {
            var projectModel = new ProjectUserViewModel();
            var project = db.Projects.Find(projectId);
            var helper = new ProjectUserHelper();
            // make sure project exists
            if(project != null)
            {
                projectModel.ProjectId = project.Id;
                projectModel.ProjectName = project.Name;
                //get the user Ids that are associated with the project
                var userIdList = helper.UsersInProject(project.Id);
                var userInfoList = getUserInfo(userIdList);
                projectModel.UsersAssignedtoProject = new MultiSelectList(userInfoList, "UserId", "UserName");

                //get the user Ids not associated with the project
                var nonUserIdList = helper.UsersNotInProject(project.Id);
                var nonUserInfoList = getUserInfo(nonUserIdList);
                projectModel.UsersNotAssignedToProject = new MultiSelectList(nonUserInfoList, "UserId", "UserName");
                return View(projectModel);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,SelectedUsers,NotSelectedUsers")] ProjectUserViewModel projModel)
        {
            var pId = projModel.ProjectId;
            var helper = new ProjectUserHelper();
            //if no users are selected, remove all users from project
            if (projModel.SelectedUsers == null)
            {
                var usersToRemove = helper.UsersInProject(pId);
                foreach(var user in usersToRemove)
                {
                    helper.RemoveUserFromProject(user, pId);
                }
            }
            else
            {
                for(var i=0; i<projModel.SelectedUsers.Length; i++)
                {
                    if(!helper.IsUserInProject(projModel.SelectedUsers[i], pId))
                    {
                        helper.AddUserToProject(projModel.SelectedUsers[i], pId);
                    }   
                }
                for(var i=0; i<projModel.NotSelectedUsers.Length; i++)
                {
                    if(helper.IsUserInProject(projModel.SelectedUsers[i], pId))
                    {
                        helper.RemoveUserFromProject(projModel.SelectedUsers[i], pId);
                    }
                }
            }
            return RedirectToAction("Index", "Projects");
        }
        public List<UserInfoViewModel> getUserInfo (List<string> userIds)
        {
            //set up a list to contain user info objects
            var userInfoList = new List<UserInfoViewModel>();

            foreach (var userId in userIds)
            {
                var myUser = new UserInfoViewModel();
                myUser.UserId = userId;
                var appUser = db.Users.Find(userId);
                myUser.FirstName = appUser.FirstName;
                myUser.LastName = appUser.LastName;
                myUser.UserName = myUser.FirstName + " " + myUser.LastName;
                userInfoList.Add(myUser);
            }
            return userInfoList;
        }
    }
}