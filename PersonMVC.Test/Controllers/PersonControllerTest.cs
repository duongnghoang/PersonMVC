using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PersonMVC.Common;
using PersonMVC.Controllers;
using PersonMVC.Responses;
using PersonMVC.Services;
using PersonMVC.ViewModels.Persons;

namespace PersonMVC.Test.Controllers;

[TestFixture]
public class PersonControllerTest
{
    private Mock<IPersonService> _personServiceMock;
    private PersonController _personController;
    private const int PAGE_INDEX = 1;
    private const int PAGE_SIZE = 10;

    [OneTimeSetUp]
    public void Setup()
    {
        // Initialize the mock service
        _personServiceMock = new Mock<IPersonService>();
        _personController = new PersonController(_personServiceMock.Object);
    }

    [Test]
    public void GetMale_ValidRequest_ReturnsViewWithPaginatedListMalePersons()
    {
        // Arrange
        var paginatedList = new PaginatedList<ListPersonViewModel>([], 0, PAGE_INDEX, PAGE_SIZE);

        _personServiceMock
            .Setup(x => x.GetPersonsMale(PAGE_INDEX, PAGE_SIZE))
            .Returns(paginatedList);

        // Act
        var result = _personController.GetMale(PAGE_INDEX, PAGE_SIZE) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetPersonsMale(PAGE_INDEX, PAGE_SIZE), Times.Once);
        
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        { 
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(_personController.ViewBag.Action, Is.Not.Null.And.EqualTo("GetMale"));
            Assert.That(result.Model, Is.EqualTo(paginatedList));
        });
    }

    [Test]
    public void Index_ValidRequest_ReturnsViewWithPaginatedListPersons()
    {
        // Arrange
        var paginatedList = new PaginatedList<ListPersonViewModel>([], 0, PAGE_INDEX, PAGE_SIZE);
        _personServiceMock
            .Setup(x => x.GetAllPersons(PAGE_INDEX, PAGE_SIZE))
            .Returns(paginatedList);

        // Act
        var result = _personController.Index(PAGE_INDEX, PAGE_SIZE) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetAllPersons(PAGE_INDEX, PAGE_SIZE), Times.Once);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(_personController.ViewBag.Action, Is.Not.Null.And.EqualTo("Index"));
            Assert.That(result.Model, Is.EqualTo(paginatedList));
        });
    }

    [Test]
    public void GetPersonsByBirthYearAction_IfActionEmpty_ReturnsBadRequestWithMessage()
    {
        // Act
        var result = _personController.GetPersonsByBirthYearAction("", PAGE_INDEX, PAGE_SIZE) as BadRequestObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(result.Value, Is.EqualTo("Action is required"));
        });
    }

    [TestCase("getbirthyearlower")]
    [TestCase("GetBirthYearLower")]
    public void GetPersonsByBirthYearAction_IfActionGetBirthYearLower_ReturnsRedirectToActionResult(string action)
    {
        // Act
        var result = _personController.GetPersonsByBirthYearAction(action, PAGE_INDEX, PAGE_SIZE) as RedirectToActionResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.RouteValues, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo("GetBirthYearLower"));
            Assert.That(result.RouteValues["pageIndex"], Is.EqualTo(PAGE_INDEX));
            Assert.That(result.RouteValues["pageSize"], Is.EqualTo(PAGE_SIZE));
        });
    }

    [TestCase("getbirthyeargreater")]
    [TestCase("GetBirthYearGreater")]
    public void GetPersonsByBirthYearAction_IfActionGetBirthYearGreater_ReturnsRedirectToActionResult(string action)
    {
        // Act
        var result = _personController.GetPersonsByBirthYearAction(action, PAGE_INDEX, PAGE_SIZE) as RedirectToActionResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.RouteValues, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo("GetBirthYearGreater"));
            Assert.That(result.RouteValues["pageIndex"], Is.EqualTo(PAGE_INDEX));
            Assert.That(result.RouteValues["pageSize"], Is.EqualTo(PAGE_SIZE));
        });
    }

    [TestCase("getbirthyearequal")]
    [TestCase("GetBirthYearEqual")]
    public void GetPersonsByBirthYearAction_IfActionGetBirthYearEqual_ReturnsRedirectToActionResult(string action)
    {
        // Act
        var result = _personController.GetPersonsByBirthYearAction(action, PAGE_INDEX, PAGE_SIZE) as RedirectToActionResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.RouteValues, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo("GetBirthYearEqual"));
            Assert.That(result.RouteValues["pageIndex"], Is.EqualTo(PAGE_INDEX));
            Assert.That(result.RouteValues["pageSize"], Is.EqualTo(PAGE_SIZE));
        });
    }

    [TestCase("invalidaction")]
    public void GetPersonsByBirthYearAction_IfActionInvalid_ReturnsNotFoundWithMessage(string action)
    {
        // Act
        var result = _personController.GetPersonsByBirthYearAction(action, PAGE_INDEX, PAGE_SIZE) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(result.Value, Is.EqualTo("Action not found"));
        });
    }

    [Test]
    public void GetBirthYearLower_ValidRequest_ReturnsViewWithPaginatedListPersons()
    {
        // Arrange
        var paginatedList = new PaginatedList<ListPersonViewModel>([], 0, PAGE_INDEX, PAGE_SIZE);
        _personServiceMock
            .Setup(x => x.GetPersonsByBirthYearWithAction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(paginatedList);

        // Act
        var result = _personController.GetBirthYearLower(PAGE_SIZE, PAGE_INDEX) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetPersonsByBirthYearWithAction("lower", PAGE_INDEX, PAGE_SIZE), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(_personController.ViewBag.Action, Is.Not.Null.And.EqualTo("BirthYearLower"));
            Assert.That(result.Model, Is.EqualTo(paginatedList));
        });
    }

    [Test]
    public void GetBirthYearGreater_ValidRequest_ReturnsViewWithPaginatedListPersons()
    {
        // Arrange
        var paginatedList = new PaginatedList<ListPersonViewModel>([], 0, PAGE_INDEX, PAGE_SIZE);
        _personServiceMock
            .Setup(x => x.GetPersonsByBirthYearWithAction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(paginatedList);

        // Act
        var result = _personController.GetBirthYearGreater(PAGE_SIZE, PAGE_INDEX) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetPersonsByBirthYearWithAction("greater", PAGE_INDEX, PAGE_SIZE), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(_personController.ViewBag.Action, Is.Not.Null.And.EqualTo("BirthYearGreater"));
            Assert.That(result.Model, Is.EqualTo(paginatedList));
        });
    }

    [Test]
    public void GetBirthYearEqual_ValidRequest_ReturnsViewWithPaginatedListPersons()
    {
        // Arrange
        var paginatedList = new PaginatedList<ListPersonViewModel>([], 0, PAGE_INDEX, PAGE_SIZE);
        _personServiceMock
            .Setup(x => x.GetPersonsByBirthYearWithAction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(paginatedList);

        // Act
        var result = _personController.GetBirthYearEqual(PAGE_SIZE, PAGE_INDEX) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetPersonsByBirthYearWithAction("equal", PAGE_INDEX, PAGE_SIZE), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(_personController.ViewBag.Action, Is.Not.Null.And.EqualTo("BirthYearEqual"));
            Assert.That(result.Model, Is.EqualTo(paginatedList));
        });
    }

    [Test]
    public void Create_Get_ReturnsPersonFormView()
    {
        // Act
        var result = _personController.Create() as ViewResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("PersonForm"));
            Assert.That(result.ViewData["Action"], Is.EqualTo("Create"));
        });
    }

    [Test]
    public void Create_PostIfModelStateIsValid_ReturnsRedirectToAction()
    {
        // Arrange
        var person = new FormPersonViewModel();
        _personServiceMock.Setup(x => x.Add(person)).Verifiable();

        // Act
        var result = _personController.Create(person) as RedirectToActionResult;

        // Assert
        _personServiceMock.Verify(x => x.Add(person), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("Index"));
    }

    [TestCase(1)]
    public void Detail_IfPersonIdExist_ReturnsDetailView(int id)
    {
        // Arrange
        var personDetail = new DetailPersonViewModel { Id = 1 };
        _personServiceMock.Setup(x => x.GetPersonDetail(id))
            .Returns(personDetail);

        // Act
        var result = _personController.Detail(id) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetPersonDetail(id), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Model, Is.EqualTo(personDetail));
        });
    }

    [TestCase(100)]
    public void Detail_IfPersonIdNotExist_ThrowsException(int id)
    {
        // Arrange
        _personServiceMock.Setup(x => x.GetPersonDetail(id))
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _personController.Detail(id));
    }

    [Test]
    public void DeleteConfirmation_Get_ReturnsView()
    {
        // Act
        var result = _personController.DeleteConfirmation() as ViewResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ViewName, Is.EqualTo("DeleteConfirmation"));
    }

    [TestCase(1)]
    public void Delete_PostIfPersonIdExist_ReturnsDeleteConfirmationView(int id)
    {
        // Arrange
        var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        _personController.TempData = tempData;
        const string DELETED_PERSON_NAME = "Deleted name";
        _personServiceMock.Setup(x => x.Delete(1))
            .Returns(DELETED_PERSON_NAME);
        const string DELETED_NAME_SUCCESS_MESSAGE = $"The person {DELETED_PERSON_NAME} was deleted successfully.";

        // Act
        var result = _personController.Delete(1) as RedirectToActionResult;

        // Assert
        _personServiceMock.Verify(x => x.Delete(1), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo("DeleteConfirmation"));
            Assert.That(_personController.TempData["SuccessMessage"],
                Is.Not.Null.And.EqualTo(DELETED_NAME_SUCCESS_MESSAGE));
        });
    }

    [TestCase(1)]
    public void Edit_GetIfPersonIdExist_ReturnsPersonFormView(int id)
    {
        // Arrange
        var personDetail = new FormPersonViewModel { Id = 1 };
        _personServiceMock.Setup(x => x.GetPersonDetailForm(id))
            .Returns(personDetail);

        // Act
        var result = _personController.Edit(id) as ViewResult;

        // Assert
        _personServiceMock.Verify(x => x.GetPersonDetailForm(id), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("PersonForm"));
            Assert.That(_personController.ViewBag.Action, Is.Not.Null.And.EqualTo("Edit"));
            Assert.That(result.Model, Is.EqualTo(personDetail));
        });
    }

    [TestCase(100)]
    public void Edit_GetIfPersonIdNotExist_ThrowsException(int id)
    {
        // Arrange
        _personServiceMock.Setup(x => x.GetPersonDetailForm(id))
            .Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _personController.Edit(id));
        _personServiceMock.Verify(x => x.GetPersonDetailForm(id), Times.Once);
    }

    [Test]
    public void Edit_PostIfModelStateIsValid_ReturnsRedirectToAction()
    {
        // Arrange
        var person = new FormPersonViewModel();
        _personServiceMock.Setup(x => x.Update(person)).Verifiable();

        // Act
        var result = _personController.Edit(person) as RedirectToActionResult;

        // Assert
        _personServiceMock.Verify(x => x.Update(person), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public void ExportToExcel_ReturnsFileResponse()
    {
        // Arrange

        var expectedBytes = new byte[] { 1, 2, 3 };
        const string EXPECTED_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        const string EXPECTED_FILENAME = "Persons_20250415120000.xlsx";
        var fileResponse = new FileResponse(expectedBytes, EXPECTED_CONTENT_TYPE, EXPECTED_FILENAME);
        _personServiceMock.Setup(x => x.ExportToExcel())
            .Returns(fileResponse);
        
        // Act
        var result = _personController.ExportToExcel() as FileContentResult;

        // Assert
        _personServiceMock.Verify(x => x.ExportToExcel(), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.FileDownloadName, Is.EqualTo(fileResponse.FileName));
            Assert.That(result.FileContents, Is.EqualTo(fileResponse.FileContent));
            Assert.That(result.ContentType, Is.EqualTo(fileResponse.ContentType));
        });
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _personController.Dispose();
    }
}