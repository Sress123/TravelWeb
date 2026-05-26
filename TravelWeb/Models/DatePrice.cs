using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class DatePrice
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public string? EndDate { get; set; }

    public string? Price { get; set; }

    public string StartDate { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Tour? Tour { get; set; }
}
