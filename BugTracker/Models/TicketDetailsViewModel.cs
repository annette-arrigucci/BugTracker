using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        //public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        //public int TicketTypeId { get; set; }
        public string TicketType { get; set; }
        //public int TicketPriorityId { get; set; }
        public string TicketPriority { get; set; }
        //public int TicketStatusId { get; set; }
        public string TicketStatus { get; set; }
        //public string OwnerUserId { get; set; }
        public string OwnerName { get; set; }
        //public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public ApplicationDbContext db = new ApplicationDbContext();

        public TicketDetailsViewModel(Ticket ticket)
        {
            this.Id = ticket.Id;
            this.Title = ticket.Title;
            this.Description = ticket.Description;
            this.Created = ticket.Created;
            this.Updated = ticket.Updated;
            //on initialization, translate Id numbers into strings for user to view
            this.ProjectTitle = getProjectTitle(ticket.ProjectId);
            this.TicketType = getType(ticket.TicketTypeId);
            this.TicketPriority = getPriority(ticket.TicketPriorityId);
            this.TicketStatus = getStatus(ticket.TicketStatusId);
            this.OwnerName = getName(ticket.OwnerUserId);
            //tickets are initially not assigned, so this field can be null
            if (!string.IsNullOrEmpty(ticket.AssignedToUserId))
            {
                this.AssignedToUserName = getName(ticket.AssignedToUserId);
            }
            else
            {
                this.AssignedToUserName = ticket.AssignedToUserId;
            }
        }

        public string getProjectTitle(int projectId)
        {
            var project = db.Projects.FirstOrDefault(x => x.Id == projectId);
            return project.Name;
        }

        public string getType(int typeId)
        {
            var tType = db.TicketTypes.FirstOrDefault(x => x.Id == typeId);
            return tType.Name;
        }

        public string getPriority(int priorityId)
        {
            var tPriority = db.TicketPriorities.FirstOrDefault(x => x.Id == priorityId);
            return tPriority.Name;
        }

        public string getStatus(int statusId)
        {
            var tStatus = db.TicketStatuses.FirstOrDefault(x => x.Id == statusId);
            return tStatus.Name;
        }

        public string getName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FirstName + " " + user.LastName;
        } 
    }
}