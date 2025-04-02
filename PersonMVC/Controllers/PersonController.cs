using Microsoft.AspNetCore.Mvc;
using PersonMVC.Models;
using PersonMVC.Services;
using PersonMVC.ViewModels.Persons;

namespace PersonMVC.Controllers;

[Route("NashTech/[controller]")]
public class PersonController(IPersonService personService) : Controller
{
    [HttpGet("Male")]
    public IActionResult GetMale(int pageIndex = 1, int pageSize = 5)
    {
        ViewBag.Action = "GetMale";
        var paginatedList = personService.GetPersonsMale(pageIndex, pageSize);
        
        return View("Index", paginatedList);
    }

    [HttpGet("Index")]
    public IActionResult Index(int pageIndex = 1, int pageSize = 5)
    {
        ViewBag.Action = "Index";
        var paginatedList = personService.GetAllPersons(pageIndex, pageSize);

        return View("Index", paginatedList);
    }

    [HttpGet("Detail")]
    public IActionResult Detail(int id)
    {
        var personDetail = personService.GetPersonDetail(id);

        return View(personDetail);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(FormPersonViewModel person)
    {
        personService.Add(person);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        ViewBag.Action = "Create";

        return View("PersonForm");
    }

    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(FormPersonViewModel person)
    {
        personService.Update(person);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var deletedPersonName = personService.Delete(id);
        TempData["SuccessMessage"] = $"The person {deletedPersonName} was deleted successfully.";
        return RedirectToAction("DeleteConfirmation");
    }

    [HttpGet]
    public IActionResult DeleteConfirmation()
    {
        return View();
    }

    [HttpGet("Edit")]
    public IActionResult Edit(int id)
    {
        ViewBag.Action = "Edit";
        var personDetail = personService.GetPersonDetailForm(id);

        return View("PersonForm", personDetail);
    }

    [HttpGet("OldestAge")]
    public Person? GetOldestPerson()
    {
        return personService.GetPersonOldestAge();
    }

    [HttpGet("FullName")]
    public IEnumerable<string> GetFullNames()
    {
        return personService.GetPersonsFullName();
    }

    [HttpGet("BirthYearWithAction")]
    public IActionResult GetPersonsByBirthYearAction([FromQuery] string action, int pageIndex = 1, int pageSize = 5)
    {
        if (string.IsNullOrEmpty(action))
        {
            return BadRequest("Action is required");
        }

        var normalizedAction = action.ToLower();
        var routeValues = new { pageIndex, pageSize };

        return normalizedAction switch
        {
            "getbirthyearlower" => RedirectToAction("GetBirthYearLower", routeValues),
            "getbirthyeargreater" => RedirectToAction("GetBirthYearGreater", routeValues),
            "getbirthyearequal" => RedirectToAction("GetBirthYearEqual", routeValues),
            _ => NotFound("Action not found"),
        };
    }

    [HttpGet("BirthYearLower")]
    public IActionResult GetBirthYearLower(int pageSize, int pageIndex)
    {
        ViewBag.Action = "BirthYearLower";
        var paginatedListBirthYearLower = personService.GetPersonsByBirthYearWithAction("lower", pageIndex, pageSize);

        return View("Index", paginatedListBirthYearLower);
    }

    [HttpGet("BirthYearGreater")]
    public IActionResult GetBirthYearGreater(int pageSize, int pageIndex)
    {
        ViewBag.Action = "BirthYearLower";
        var paginatedListBirthYearGreater = personService.GetPersonsByBirthYearWithAction("greater", pageIndex, pageSize);

        return View("Index", paginatedListBirthYearGreater);
    }

    [HttpGet("BirthYearEqual")]
    public IActionResult GetBirthYearEqual(int pageSize, int pageIndex)
    {
        ViewBag.Action = "BirthYearLower";
        var paginatedListBirthYearGreater = personService.GetPersonsByBirthYearWithAction("equal", pageIndex, pageSize);

        return View("Index", paginatedListBirthYearGreater);
    }

    [HttpPost("ExportToExcel")]
    public IActionResult ExportToExcel()
    {
        var fileResponse = personService.ExportToExcel();

        return File(fileResponse.FileContent, fileResponse.ContentType, fileResponse.FileName);
    }
}
