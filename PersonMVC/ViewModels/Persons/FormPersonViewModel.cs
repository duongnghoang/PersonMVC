using PersonMVC.Common.Enums;

namespace PersonMVC.ViewModels.Persons;

public class FormPersonViewModel
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? BirthPlace { get; set; }
    public bool IsGraduated { get; set; }
}