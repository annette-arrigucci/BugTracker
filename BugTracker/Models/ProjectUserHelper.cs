using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectUserHelper
    {
        private ApplicationDbContext db;

        public ProjectUserHelper()
        {
            this.db = new ApplicationDbContext();
        }
        public bool IsUserInProject(string userId, int projectId)
        {
            var query1 = db.ProjectUsers.Where(c => c.ProjectId == projectId);
            var query2 = query1.Where(u => u.UserId.Equals(userId));
            if(query2.ToList().Count > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IList<string> ListUserProjects(string userId)
        {
            var projQuery = db.ProjectUsers.Where(u => u.UserId.Equals(userId));
            var projList = projQuery.ToList();
            var projectsList = new List<string>();

            if (projList.Count > 0)
            {   
                foreach (var project in projList)
                {
                    //var titleQuery = db.Projects.Where(p => p.Id.Equals(project));
                    var query = from p in db.Projects
                                where p.Id == project.Id
                                select p.Name;
                    foreach (var a in query)
                    {
                        projectsList.Add(a);
                    }
                }  
            }
            return projectsList;
        }
        public bool AddUserToProject(string userId, int projectId)
        {
            ProjectUser pUser = new ProjectUser { UserId = userId, ProjectId = projectId };
            db.ProjectUsers.Add(pUser);
            var result = db.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserInProject(userId, projectId))
            {
                var deleteEntries =
                from pUsers in db.ProjectUsers
                where pUsers.UserId == userId && pUsers.ProjectId == projectId
                select pUsers;

                foreach (var entry in deleteEntries)
                {
                    var delete = db.ProjectUsers.Remove(entry);
                    //db.ProjectUsers.DeleteOnSubmit(entry);
                }
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}