using PersonMVC.Common;
using PersonMVC.Common.Data;
using PersonMVC.Common.Enums;
using PersonMVC.Common.Helpers;
using PersonMVC.Models;
using PersonMVC.Responses;
using PersonMVC.ViewModels.Persons;

namespace PersonMVC.Services;

public class PersonService(PersonData personData) : IPersonService
{
    public void Add(FormPersonViewModel personModel)
    {
        var newId = personData.Persons.Max(p => p.Id) + 1;
        var person = new Person(newId, personModel.FirstName, personModel.LastName, personModel.Gender,
            personModel.DateOfBirth, personModel.PhoneNumber, personModel.BirthPlace, personModel.IsGraduated);

        personData.Persons.Add(person);
    }

    public PaginatedList<ListPersonViewModel> GetAllPersons(int pageIndex, int pageSize)
    {
        var persons = personData.Persons
            .Select(p => new ListPersonViewModel
            {
                Id = p.Id,
                FullName = p.FullName,
                BirthPlace = p.BirthPlace,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                GraduatedString = p.IsGraduated ? "Yes" : "No"
            })
            .ToList();

        return PaginatedList<ListPersonViewModel>.Create(persons, pageIndex, pageSize);
    }

    public string Delete(int id)
    {
        var deletedPerson = personData.Persons.First(p => p.Id == id);
        personData.Persons.Remove(deletedPerson);

        return $"{deletedPerson.FirstName} {deletedPerson.LastName}";
    }

    public void Update(FormPersonViewModel personModel)
    {
        var updatedPerson = personData.Persons.First(p => p.Id == personModel.Id);
        updatedPerson.FirstName = personModel.FirstName;
        updatedPerson.LastName = personModel.LastName;
        updatedPerson.Gender = personModel.Gender;
        updatedPerson.DateOfBirth = personModel.DateOfBirth;
        updatedPerson.PhoneNumber = personModel.PhoneNumber;
        updatedPerson.BirthPlace = personModel.BirthPlace;
        updatedPerson.IsGraduated = personModel.IsGraduated;
    }

    public PaginatedList<ListPersonViewModel> GetPersonsMale(int pageIndex, int pageSize)
    {
        var malePersons = personData.Persons
            .Select(p => new ListPersonViewModel
            {
                Id = p.Id,
                FullName = p.FullName,
                BirthPlace = p.BirthPlace,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                GraduatedString = p.IsGraduated ? "Yes" : "No"
            })
            .Where(mp => mp.Gender == Gender.Male)
            .ToList();

        return PaginatedList<ListPersonViewModel>.Create(malePersons, pageIndex, pageSize);
    }

    public Person GetPersonOldestAge()
    {
        var oldestPerson = personData.Persons.OrderByDescending(p => p.DateOfBirth).Last();

        return oldestPerson;
    }

    public IEnumerable<string> GetPersonsFullName()
    {
        var fullNames = personData.Persons.Select(p => p.FullName);

        return fullNames;
    }

    public PaginatedList<ListPersonViewModel> GetPersonsByBirthYearWithAction(string action, int pageIndex, int pageSize)
    {
        var persons = personData.Persons
            .Select(p => new ListPersonViewModel
            {
                Id = p.Id,
                FullName = p.FullName,
                BirthPlace = p.BirthPlace,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                GraduatedString = p.IsGraduated ? "Yes" : "No"
            });

        var filteredPersons = action switch
        {
            "greater" => persons.Where(p => p.DateOfBirth.Year > 2000),
            "lower" => persons.Where(p => p.DateOfBirth.Year < 2000),
            "equal" => persons.Where(p => p.DateOfBirth.Year == 2000),
            _ => []
        };

        return PaginatedList<ListPersonViewModel>.Create(filteredPersons.ToList(), pageIndex, pageSize);
    }

    public FileResponse ExportToExcel()
    {
        var personToExcel = personData.Persons.ToList();
        var fileContent = FileHelper<Person>.GenerateExcelFile(personToExcel);
        var fileName = $"Persons_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

        return new FileResponse(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    public DetailPersonViewModel GetPersonDetail(int id)
    {
        var detailPerson = personData.Persons
            .Select(p => new DetailPersonViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthPlace = p.BirthPlace,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                GraduatedString = p.IsGraduated ? "Yes" : "No"
            }).First(p => p.Id == id);

        return detailPerson;
    }

    public FormPersonViewModel GetPersonDetailForm(int id)
    {
        var formPerson = personData.Persons
            .Select(p => new FormPersonViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthPlace = p.BirthPlace,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                IsGraduated = p.IsGraduated
            }).First(p => p.Id == id);

        return formPerson;
    }
}