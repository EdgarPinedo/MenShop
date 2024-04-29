using System;
using System.Collections.Generic;

namespace Online_Shop.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string States { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
