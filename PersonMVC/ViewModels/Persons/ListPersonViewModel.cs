using PersonMVC.Common.Enums;

namespace PersonMVC.ViewModels.Persons;

public class ListPersonViewModel
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? BirthPlace { get; set; }
    public string GraduatedString { get; init; } = null!;
}