using ClassLibrary.Models;
using ClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjApplication.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IMilitaryUnitService _militaryUnitService;
        private readonly IVolunteerService _volunteerService;
        private readonly IOrganizationService _organizationService;

        public RequestController(
            IRequestService requestService,
            IMilitaryUnitService militaryUnitService,
            IVolunteerService volunteerService,
            IOrganizationService organizationService)
        {
            _requestService = requestService;
            _militaryUnitService = militaryUnitService;
            _volunteerService = volunteerService;
            _organizationService = organizationService;
        }

        // GET: Request
        public async Task<IActionResult> Index()
        {
            var requests = await _requestService.GetAllAsync();
            return View(requests);
        }

        // GET: Request/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            var militaryUnit = await _militaryUnitService.GetByIdAsync(request.MilitaryUnitId);
            ViewBag.MilitaryUnit = militaryUnit;

            if (request.TakenByVolunteerId.HasValue)
            {
                ViewBag.TakenByVolunteer = await _volunteerService.GetByIdAsync(request.TakenByVolunteerId.Value);
            }

            if (request.OrganizationTakenById.HasValue)
            {
                ViewBag.TakenByOrganization = await _organizationService.GetByIdAsync(request.OrganizationTakenById.Value);
            }

            if (request.CompletedByVolunteerId.HasValue)
            {
                ViewBag.CompletedByVolunteer = await _volunteerService.GetByIdAsync(request.CompletedByVolunteerId.Value);
            }

            if (request.OrganizationCompletedById.HasValue)
            {
                ViewBag.CompletedByOrganization = await _organizationService.GetByIdAsync(request.OrganizationCompletedById.Value);
            }

            return View(request);
        }

        // POST: Request/Take/5
        [HttpPost]
        public async Task<IActionResult> TakeRequestAsVolunteer(int id, int volunteerId)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.TakenByVolunteerId = volunteerId;
            await _requestService.UpdateAsync(request);

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Request/Take/Organization/5
        [HttpPost]
        public async Task<IActionResult> TakeRequestAsOrganization(int id, int organizationId)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.OrganizationTakenById = organizationId;
            await _requestService.UpdateAsync(request);

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Request/Complete/5
        [HttpPost]
        public async Task<IActionResult> CompleteRequestAsVolunteer(int id, int volunteerId)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.CompletedByVolunteerId = volunteerId;
            request.Status = "Completed";
            await _requestService.UpdateAsync(request);

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Request/Complete/Organization/5
        [HttpPost]
        public async Task<IActionResult> CompleteRequestAsOrganization(int id, int organizationId)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.OrganizationCompletedById = organizationId;
            request.Status = "Completed";
            await _requestService.UpdateAsync(request);

            return RedirectToAction(nameof(Details), new { id });
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var militaryUnits = await _militaryUnitService.GetAllAsync();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestEntity request)
        {
            await _requestService.AddAsync(request);
            return RedirectToAction(nameof(Index));

        }


        // POST: Request/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            await _requestService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
