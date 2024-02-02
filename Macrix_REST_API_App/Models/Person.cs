using System;
using System.Collections.Generic;

namespace Macrix_REST_API_App.Models;

public partial class Person
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public long HouseNumber { get; set; }

    public long? ApartmentNumber { get; set; }

    public string PostalCode { get; set; } = null!;

    public string Town { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public string DateOfBirth { get; set; } = null!;
}
