using System;
using System.Collections.Generic;

namespace Online_Shop.Models;

public partial class CartItem
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public string Size { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
