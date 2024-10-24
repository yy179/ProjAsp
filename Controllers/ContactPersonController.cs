using ClassLibrary.Models;
using ClassLibrary.Services;
using ClassLibrary.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjApplication.Controllers
{
    public class ContactPersonController : Controller
    {
        private readonly IMilitaryUnitService _militaryUnitService;
        private readonly IContactPersonService _contactPersonService;
        public ContactPersonController(IMilitaryUnitService militaryUnitService, IContactPersonService contactPersonService)
        {
            _militaryUnitService = militaryUnitService;
            _contactPersonService = contactPersonService;
        }
        // GET: ContactPersonController
        public async Task<IActionResult> Index()
        {
            var contactPersons = await _contactPersonService.GetAllAsync();
            return View(contactPersons);
        }

        // GET: MilitaryUnitController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contactPerson = await _contactPersonService.GetByIdAsync(id);
            if (contactPerson == null)
            {
                return NotFound();
            }

            return View(contactPerson);
        }


        // GET: ContactPersonController/Create
        public async Task<ActionResult> Create()
        {
            var militaryUnits = await _militaryUnitService.GetAllAsync();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            return View();
        }

        // POST: ContactPersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactPersonEntity contactPerson)
        {
            ModelState.Remove("MilitaryUnit");
            if (ModelState.IsValid)
            {
                try
                {
                    await _contactPersonService.AddAsync(contactPerson);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            var militaryUnits = await _militaryUnitService.GetAllAsync();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            return View(contactPerson);
        }


        // GET: ContactPersonController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contactPerson = await _contactPersonService.GetByIdAsync(id);
            if (contactPerson == null)
            {
                return NotFound();
            }
            return View(contactPerson);
        }

        // POST: ContactPersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactPersonEntity contactPerson)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _contactPersonService.UpdateAsync(contactPerson);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(contactPerson);
        }

        // GET: ContactPersonController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contactPerson = await _contactPersonService.GetByIdAsync(id);
            if (contactPerson == null)
            {
                return NotFound();
            }
            return View(contactPerson);
        }

        // POST: MilitaryUnitController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _contactPersonService.DeleteAsync(id);
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
