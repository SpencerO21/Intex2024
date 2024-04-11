using System;
using System.Collections.Generic;

namespace Intex2024.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string CountryOfResidence { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int Age { get; set; }

    public int RelatedItem1 { get; set; }

    public int RelatedItem2 { get; set; }

    public int RelatedItem3 { get; set; }

    public string? UserId { get; set; }
}
