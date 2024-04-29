using System;
using System.Collections.Generic;

namespace Online_Shop.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Total { get; set; }

    public string Status { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual User User { get; set; } = null!;
}
