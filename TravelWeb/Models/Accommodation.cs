using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class Accommodation
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public string? Location { get; set; }

    public string? Type { get; set; }

    public int? NoOfNight { get; set; }

    public string? Abbreviation { get; set; }

    public virtual Tour? Tour { get; set; }
}
