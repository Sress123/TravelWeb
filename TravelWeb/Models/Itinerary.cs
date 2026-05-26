using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class Itinerary
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public int DayNumber { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual Tour? Tour { get; set; }
}
