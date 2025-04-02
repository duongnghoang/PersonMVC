using PersonMVC.Common;
using PersonMVC.Models;
using PersonMVC.Responses;
using PersonMVC.ViewModels.Persons;

namespace PersonMVC.Services;

public interface IPersonService
{
    PaginatedList<ListPersonViewModel> GetPersonsMale(int pageIndex, int pageSize);
    Person GetPersonOldestAge();
    IEnumerable<string> GetPersonsFullName();
    PaginatedList<ListPersonViewModel> GetPersonsByBirthYearWithAction(string action, int pageIndex, int pageSize);
    FileResponse ExportToExcel();
    void Add(FormPersonViewModel person);
    PaginatedList<ListPersonViewModel> GetAllPersons(int pageIndex, int pageSize);
    string Delete(int id);
    void Update(FormPersonViewModel person);
    DetailPersonViewModel GetPersonDetail(int id);
    FormPersonViewModel GetPersonDetailForm(int id);
}