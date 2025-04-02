using PersonMVC.Common.Enums;

namespace PersonMVC.Models;

public class Person(
    int id,
    string? firstName,
    string? lastName,
    Gender gender,
    DateTime dateOfBirth,
    string? phoneNumber,
    string? birthPlace,
    bool isGraduated)
{
    public int Id { get; internal set; } = id;
    public string? FirstName { get; internal set; } = firstName;
    public string? LastName { get; internal set; } = lastName;
    public string FullName => $"{FirstName} {LastName}";
    public Gender Gender { get; internal set; } = gender;
    public DateTime DateOfBirth { get; internal set; } = dateOfBirth;
    public string? PhoneNumber { get; internal set; } = phoneNumber;
    public string? BirthPlace { get; internal set; } = birthPlace;
    public bool IsGraduated { get; internal set; } = isGraduated;
}