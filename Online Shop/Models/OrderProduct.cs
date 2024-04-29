using System;
using System.Collections.Generic;

namespace Online_Shop.Models;

public partial class OrderProduct
{
    public Guid ProductId { get; set; }

    public Guid OrderId { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
