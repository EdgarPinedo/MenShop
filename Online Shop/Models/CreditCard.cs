using System;
using System.Collections.Generic;

namespace Online_Shop.Models;

public partial class CreditCard
{
    public Guid Id { get; set; }

    public string Number { get; set; } = null!;

    public string ExpirationMonth { get; set; } = null!;

    public string ExpirationYear { get; set; } = null!;

    public string Cvv { get; set; } = null!;

    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
