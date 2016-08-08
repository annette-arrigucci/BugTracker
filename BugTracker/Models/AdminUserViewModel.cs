using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

public class AdminUserViewModel
{
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public MultiSelectList Roles { get; set; }
    //public List<string> SelectedRoles { get; set; }
    public List<string> SelectedRoles { get; set; }
    public string RolesList { get; set; }
}