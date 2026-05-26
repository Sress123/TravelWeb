using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public int? DatePriceId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? Country { get; set; }

    public string? Address { get; set; }

    public int? NoOfTravellers { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual DatePrice? DatePrice { get; set; }

    public virtual Tour? Tour { get; set; }
}
