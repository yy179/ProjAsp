using ClassLibrary.Models;
using ClassLibrary.Services;
using ClassLibrary.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjApplication.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IVolunteerOrganizationService _volunteerOrganizationService;
        private readonly IVolunteerService _volunteerService;
        private readonly IOrganizationService _organizationService;

        public OrganizationController(
            IVolunteerOrganizationService volunteerOrganizationService,
            IVolunteerService volunteerService,
            IOrganizationService organizationService)
        {
            _volunteerOrganizationService = volunteerOrganizationService;
            _volunteerService = volunteerService;
            _organizationService = organizationService;
        }
        // GET: OrganizationController
        public async Task<IActionResult> Index()
        {
            var organizations = await _organizationService.GetAllAsync();
            return View(organizations);
        }

        // GET: OrganizationController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var organization = await _organizationService.GetByIdAsync(id);
            var volunteers = await _organizationService.GetVolunteersAsync(id);
            ViewBag.Volunteers = volunteers;
            var requests = await _organizationService.GetActiveRequestsAsync(id);
            ViewBag.Requests = requests;
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // GET: OrganizationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrganizationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationEntity organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _organizationService.AddAsync(organization);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(organization);
        }

        // GET: OrganizationController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var organization = await _organizationService.GetByIdAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: OrganizationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrganizationEntity organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _organizationService.UpdateAsync(organization);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(organization);
        }

        // GET: OrganizationController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var organization = await _organizationService.GetByIdAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: OrganizationController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _organizationService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveVolunteer(int volunteerId, int organizationId)
        {
            try
            {
                await _volunteerOrganizationService.RemoveVolunteerFromOrganizationAsync(volunteerId, organizationId);
                return RedirectToAction(nameof(Details), new { id = organizationId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var organization = await _organizationService.GetByIdAsync(organizationId);
                var volunteers = await _organizationService.GetVolunteersAsync(organizationId);
                ViewBag.Volunteers = volunteers;
                var requests = await _organizationService.GetActiveRequestsAsync(organizationId);
                ViewBag.Requests = requests;
                return View("Details", organization);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Details), new { id = organizationId });
            }
        }

    }
}
