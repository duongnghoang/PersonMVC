using PersonMVC.Common.Data;
using PersonMVC.Common.Enums;
using PersonMVC.Models;
using PersonMVC.Responses;
using PersonMVC.Services;
using PersonMVC.ViewModels.Persons;

namespace PersonMVC.Test.Services;

[TestFixture]
public class PersonServiceTests
{
    private PersonService _personService;
    private const int PAGE_INDEX = 1;
    private const int PAGE_SIZE = 10;

    [SetUp]
    public void Setup()
    {
        // Setup code here
        var personData = new PersonData
        {
            Persons =
            [
                new Person(1, "ABC", "Doe", Gender.Male, new DateTime(1999, 1, 21), "123-654-8799", "New York", true),
                new Person(2, "DEF", "Doe", Gender.Female, new DateTime(2001, 12, 2), "254-367-9801", "Los Angeles", false),
                new Person(3, "GHI", "Smith", Gender.Male, new DateTime(1995, 6, 15), "345-678-1234", "Chicago", true),
                new Person(4, "JKL", "Johnson", Gender.Female, new DateTime(1998, 10, 30), "456-789-2345", "Houston", false),
                new Person(5, "MNO", "Brown", Gender.Male, new DateTime(2000, 7, 18), "567-890-3456", "Phoenix", true),
                new Person(6, "PQR", "Davis", Gender.Female, new DateTime(2002, 3, 5), "678-901-4567", "Philadelphia", false),
                new Person(7, "STU", "Wilson", Gender.Male, new DateTime(1997, 8, 25), "789-012-5678", "San Francisco", true)
            ]
        };

        _personService = new PersonService(personData);
    }

    [Test]
    public void Add_ValidRequest_ReturnsNewPerson()
    {
        // Arrange
        var personModel = new FormPersonViewModel
        {
            FirstName = "Jane",
            LastName = "Doe",
            PhoneNumber = "123-456-7890",
            Gender = Gender.Female,
            DateOfBirth = new DateTime(1992, 2, 2),
            BirthPlace = "Los Angeles",
            IsGraduated = true
        };

        // Act
        _personService.Add(personModel);
        var personList = _personService.GetAllPersons(PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(personList, Has.Count.EqualTo(8));
            Assert.That(personList[7].Id, Is.EqualTo(8));
        });
    }

    [Test]
    public void GetAllPersons_ReturnsPaginatedList()
    {
        // Act
        var result = _personService.GetAllPersons(PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(7));
        });
    }

    [TestCase(1)]
    public void Delete_IfPersonIdExist_ReturnsDeletedPersonName(int id)
    {
        // Arrange
        const string EXPECTED_NAME = "ABC Doe";

        // Act
        var deletedPerson = _personService.Delete(id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(deletedPerson, Is.EqualTo(EXPECTED_NAME));
            var personList = _personService.GetAllPersons(PAGE_INDEX, PAGE_SIZE);
            Assert.That(!personList.Exists(p => p.Id == id));
        });
    }

    [TestCase(100)]
    public void Delete_IfPersonIdNotExist_ThrowsException(int id)
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _personService.Delete(id), "No person found with the given ID.");
    }

    [TestCase(1)]
    public void Update_IfPersonIdExist_UpdatePersonData(int id)
    {
        // Arrange
        var personModel = new FormPersonViewModel
        {
            Id = id,
            FirstName = "Jane",
            LastName = "Doe",
            Gender = Gender.Female,
            PhoneNumber = "123-456-1234",
            DateOfBirth = new DateTime(1992, 2, 2),
            BirthPlace = "Los Angeles",
            IsGraduated = true
        };

        // Act
        _personService.Update(personModel);

        // Assert
        var updatedPerson = _personService
            .GetAllPersons(PAGE_INDEX, PAGE_SIZE)
            .First(p => p.Id == id);

        Assert.Multiple(() =>
        {
            Assert.That(updatedPerson.FullName, Is.EqualTo("Jane Doe"));
            Assert.That(updatedPerson.Gender, Is.EqualTo(Gender.Female));
            Assert.That(updatedPerson.PhoneNumber, Is.EqualTo("123-456-1234"));
            Assert.That(updatedPerson.DateOfBirth, Is.EqualTo(new DateTime(1992, 2, 2)));
            Assert.That(updatedPerson.BirthPlace, Is.EqualTo("Los Angeles"));
            Assert.That(updatedPerson.GraduatedString, Is.EqualTo("Yes"));
        });
    }

    [TestCase(100)]
    public void Update_IfPersonNotExist_ThrowsException(int id)
    {
        // Arrange
        var personModel = new FormPersonViewModel
        {
            Id = id,
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _personService.Update(personModel),  "No person found with the given ID.");
    }

    [TestCase("greater")]
    public void GetPersonByBirthYearWithAction_ActionIsGreater_ReturnsDOBYearGreater2000(string action)
    {
        // Act
        var result = _personService.GetPersonsByBirthYearWithAction(action, PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Select(p => p.DateOfBirth.Year), Is.All.GreaterThan(2000));
        });
        
    }

    [TestCase("lower")]
    public void GetPersonByBirthYearWithAction_ActionIsLess_ReturnsDOBYearLess2000(string action)
    {
        // Act
        var result = _personService.GetPersonsByBirthYearWithAction(action, PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(4));
            Assert.That(result.Select(p => p.DateOfBirth.Year), Is.All.LessThan(2000));
        });
    }

    [TestCase("equal")]
    public void GetPersonByBirthYearWithAction_ActionIsEqual_ReturnsDOBYearEqual2000(string action)
    {
        // Act
        var result = _personService.GetPersonsByBirthYearWithAction(action, PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.Select(p => p.DateOfBirth.Year), Is.All.EqualTo(2000));
        });
    }

    [TestCase("invalid")]
    public void GetPersonByBirthYearWithAction_ActionIsNotValid_ReturnsEmptyList(string action)
    {
        // Act
        var result = _personService.GetPersonsByBirthYearWithAction(action, PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.That(result, Has.Count.Zero);
    }

    [Test]
    public void GetPersonsMale_ReturnsMalePersons()
    {
        // Arrange
        const string GENDER_PROPERTY = "Gender";

        // Act
        var result = _personService.GetPersonsMale(PAGE_INDEX, PAGE_SIZE);

        // Assert
        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result, Has.All.Property(GENDER_PROPERTY).EqualTo(Gender.Male));
    }

    [Test]
    public void GetOldestAge_ReturnsOldestPerson()
    {
        // Act
        var result = _personService.GetPersonOldestAge();

        // Assert
        Assert.That(result.Id, Is.EqualTo(3));
    }

    [Test]
    public void ExportToExcel_ReturnsFileResponse()
    {
        // Arrange
        const string EXCEL_FILE_NAME_PREFIX = "Persons_";
        const string EXCEL_FILE_NAME_EXTENSION = ".xlsx";
        const string EXCEL_FILE_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // Act
        var result = _personService.ExportToExcel();
        var fileName = result.FileName;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<FileResponse>());
            Assert.That(fileName, Does.StartWith(EXCEL_FILE_NAME_PREFIX));
            Assert.That(fileName, Does.EndWith(EXCEL_FILE_NAME_EXTENSION));
            Assert.That(result.ContentType, Is.EqualTo(EXCEL_FILE_CONTENT_TYPE));
            Assert.That(result.FileContent, Is.Not.Null);
        });
        
    }

    [TestCase(1)]
    public void GetPersonDetail_IfPersonIdExist_ReturnsPersonDetail(int id)
    {
        // Arrange
        const string EXPECTED_FIRST_NAME = "ABC";

        // Act
        var result = _personService.GetPersonDetail(id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.FirstName, Is.EqualTo(EXPECTED_FIRST_NAME));
        });
    }

    [TestCase(100)]
    public void GetPersonDetail_IfPersonIdNotExist_ThrowsException(int id)
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _personService.GetPersonDetail(id), "No person found with the given ID.");
    }

    [Test]
    public void GetPersonsFullName_ReturnsFullNames()
    {
        // Arrange
        var expectedFullNames = new List<string>
        {
            "ABC Doe",
            "DEF Doe",
            "GHI Smith",
            "JKL Johnson",
            "MNO Brown",
            "PQR Davis",
            "STU Wilson"
        };

        // Act
        var result = _personService.GetPersonsFullName();

        // Assert
        Assert.That(result, Is.EqualTo(expectedFullNames));
    }

    [TestCase(1)]
    public void GetPersonDetailForm_IfPersonIdExist_ReturnFormPerson(int id)
    {
        // Act
        var result = _personService.GetPersonDetailForm(1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        });
    }

    [TestCase(100)]
    public void GetPersonDetailForm_IfPersonIdNotExist_ThrowsException(int id)
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _personService.GetPersonDetailForm(id), "No person found with the given ID.");
    }
}