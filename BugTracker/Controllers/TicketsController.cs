﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            return View(db.Tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            var ticketView = new TicketCreateViewModel();
            ticketView.Projects = new SelectList(db.Projects, "Id", "Name");
            ticketView.TicketTypes = new SelectList(db.TicketTypes, "Id", "Name");
            ticketView.TicketPriorities = new SelectList(db.TicketPriorities, "Id", "Name");
            //ticketView.TicketStatuses = new SelectList(db.TicketStatuses, "Id", "Name");
            return View(ticketView);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,SelectedProject,SelectedType,SelectedPriority")] TicketCreateViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var ticket = new Ticket();
                ticket.Title = tvm.Title;
                ticket.Description = tvm.Description;
                ticket.Created = DateTimeOffset.Now;
                ticket.ProjectId = tvm.SelectedProject;
                ticket.TicketTypeId = tvm.SelectedType;
                ticket.TicketPriorityId = tvm.SelectedPriority;
                var query = from p in db.TicketStatuses
                            where p.Name == "New"
                            select p.Id;
                ticket.TicketStatusId = query.First();
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tvm);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            else
            {
                var ticketEdit = new TicketEditViewModel();
                ticketEdit.Id = ticket.Id;
                ticketEdit.Title = ticket.Title;
                ticketEdit.Created = ticket.Created;
                ticketEdit.Updated = ticket.Updated;
                ticketEdit.Description = ticket.Description;
                
                //setting the default selected values of the TicketEditViewModel to the current values in the ticket
                ticketEdit.SelectedProject = ticket.ProjectId;
                ticketEdit.SelectedType = ticket.TicketTypeId;
                ticketEdit.SelectedPriority = ticket.TicketPriorityId;
                ticketEdit.SelectedStatus = ticket.TicketStatusId;
                ticketEdit.OwnerUserId = ticket.OwnerUserId;
                ticketEdit.AssignedToUserId = ticket.AssignedToUserId;

                ticketEdit.Projects = new SelectList(db.Projects, "Id", "Name", ticketEdit.SelectedProject);//ticket.ProjectId
                ticketEdit.TicketTypes = new SelectList(db.TicketTypes, "Id", "Name", ticketEdit.SelectedType);//ticket.TicketTypeId
                ticketEdit.TicketPriorities = new SelectList(db.TicketPriorities, "Id", "Name", ticketEdit.SelectedPriority);//ticket.TicketPriorityId
                ticketEdit.TicketStatuses = new SelectList(db.TicketStatuses, "Id", "Name", ticketEdit.SelectedStatus);//ticket.TicketStatusId

                return View(ticketEdit);
            }
            
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,SelectedProject,SelectedType,SelectedPriority,SelectedStatus,OwnerUserId,AssignedToUserId")] TicketEditViewModel tevModel)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = db.Tickets.Find(tevModel.Id);
                ticket.Title = tevModel.Title;
                ticket.Description = tevModel.Description;
                ticket.Created = tevModel.Created;
                ticket.Updated = DateTimeOffset.Now;
                ticket.ProjectId = tevModel.SelectedProject;
                ticket.TicketTypeId = tevModel.SelectedType;
                ticket.TicketPriorityId = tevModel.SelectedPriority;
                ticket.TicketStatusId = tevModel.SelectedStatus;
                ticket.OwnerUserId = tevModel.OwnerUserId;
                ticket.AssignedToUserId = tevModel.AssignedToUserId;

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tevModel);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
