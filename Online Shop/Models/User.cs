using System;
using System.Collections.Generic;

namespace Online_Shop.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
