using PersonMVC.Common.Enums;
using PersonMVC.Models;

namespace PersonMVC.Common.Data;

public class PersonData
{
    public List<Person> Persons { get; init; } =
    [
        new(1, "John", "Doe", Gender.Male, new DateTime(1990, 1, 1), "123-456-7890", "New York", true),
        new(2, "Jane", "Doe", Gender.Female, new DateTime(2000, 2, 2), "234-567-8901", "Los Angeles", false),
        new(3, "Jim", "Beam", Gender.Male, new DateTime(1985, 3, 3), "345-678-9012", "Chicago", true),
        new(4, "Jack", "Daniels", Gender.Male, new DateTime(2000, 4, 4), "456-789-0123", "Houston", true),
        new(5, "Jill", "Valentine", Gender.Female, new DateTime(1995, 5, 5), "567-890-1234", "Phoenix", false),
        new(6, "Chris", "Redfield", Gender.Other, new DateTime(2022, 6, 6), "678-901-2345", "Philadelphia", true),
        new(7, "Claire", "Redfield", Gender.Female, new DateTime(1991, 7, 7), "789-012-3456", "San Antonio", true),
        new(8, "Leon", "Kennedy", Gender.Male, new DateTime(2021, 8, 8), "890-123-4567", "San Diego", false),
        new(9, "Ethan", "Winters", Gender.Male, new DateTime(1993, 9, 9), "901-234-5678", "Miami", true),
        new(10, "Carlos", "Oliveira", Gender.Male, new DateTime(1988, 10, 10), "012-345-6789", "Dallas", false),
        new(11, "Barry", "Burton", Gender.Male, new DateTime(1975, 11, 11), "123-456-7891", "Austin", true),
        new(12, "Albert", "Wesker", Gender.Male, new DateTime(1960, 12, 12), "234-567-8902", "Seattle", false),
        new(13, "William", "Birkin", Gender.Male, new DateTime(1965, 1, 13), "345-678-9013", "Denver", true),
        new(14, "Hunk", "Unknown", Gender.Male, new DateTime(1980, 2, 14), "456-789-0124", "Las Vegas", true),
        new(15, "Jake", "Muller", Gender.Male, new DateTime(1992, 3, 15), "567-890-1235", "Portland", false),
        new(16, "Piers", "Nivans", Gender.Male, new DateTime(1990, 4, 16), "678-901-2346", "Orlando", true),
        new(17, "Josh", "Stone", Gender.Male, new DateTime(1985, 5, 17), "789-012-3457", "San Francisco", false),
        new(18, "Robert", "Kendo", Gender.Male, new DateTime(1970, 6, 18), "890-123-4568", "Boston", true)
    ];
}