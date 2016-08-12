using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketAssignViewModel
    {
        public Ticket Ticket { get; set; }
        public SelectList ProjUsersList { get; set; }
        public string SelectedUser { get; set; }
    }
}