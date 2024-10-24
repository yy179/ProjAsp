using ClassLibrary.Models;
using ClassLibrary.Repositories.interfaces;
using ClassLibrary.Services;
using ClassLibrary.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjApplication.Controllers
{
    public class MilitaryUnitController : Controller
    {
        private readonly IMilitaryUnitService _militaryUnitService;
        private readonly IContactPersonService _contactPersonService;
        public MilitaryUnitController(IMilitaryUnitService militaryUnitService, IContactPersonService contactPersonService)
        {
            _militaryUnitService = militaryUnitService;
            _contactPersonService = contactPersonService;
        }
        // GET: MilitaryUnitController
        public async Task<IActionResult> Index()
        {
            var militaryUnits = await _militaryUnitService.GetAllAsync();
            return View(militaryUnits);
        }

        // GET: MilitaryUnitController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contactPersons = await _contactPersonService.GetByMilitaryUnitId(id);
            var militaryUnit = await _militaryUnitService.GetByIdAsync(id);
            var requests = await _militaryUnitService.GetActiveRequests(id);
            ViewBag.Requests = requests;
            ViewBag.ContactPersons = contactPersons;
            if (militaryUnit == null)
            {
                return NotFound();
            }
            return View(militaryUnit);
        }

        // GET: MilitaryUnitController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MilitaryUnitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MilitaryUnitEntity militaryUnit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _militaryUnitService.AddAsync(militaryUnit);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(militaryUnit);
        }

        // GET: MilitaryUnitController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var militaryUnit = await _militaryUnitService.GetByIdAsync(id);
            if (militaryUnit == null)
            {
                return NotFound();
            }
            return View(militaryUnit);
        }

        // POST: MilitaryUnitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MilitaryUnitEntity militaryUnit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _militaryUnitService.UpdateAsync(militaryUnit);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(militaryUnit);
        }

        // GET: MilitaryUnitController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var militaryUnit = await _militaryUnitService.GetByIdAsync(id);
            if (militaryUnit == null)
            {
                return NotFound();
            }
            return View(militaryUnit);
        }

        // POST: MilitaryUnitController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _militaryUnitService.DeleteAsync(id);
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
