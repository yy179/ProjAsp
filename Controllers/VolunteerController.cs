﻿using ClassLibrary.Models;
using ClassLibrary.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjApplication.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IVolunteerService _volunteerService;

        public VolunteerController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }

        // GET: Volunteer/Index
        public async Task<IActionResult> Index()
        {
            var volunteers = await _volunteerService.GetAllAsync();
            return View(volunteers);
        }

        // GET: Volunteer/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            var organizations = await _volunteerService.GetOrganizations(id);
            ViewBag.Organizations = organizations;
            var requests = await _volunteerService.GetActiveRequests(id);
            ViewBag.Requests = requests;
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // GET: Volunteer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerEntity volunteer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _volunteerService.AddAsync(volunteer);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(volunteer);
        }

        // GET: Volunteer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VolunteerEntity volunteer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _volunteerService.UpdateAsync(volunteer);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(volunteer);
        }

        // GET: Volunteer/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var volunteer = await _volunteerService.GetByIdAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _volunteerService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}