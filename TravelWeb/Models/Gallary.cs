using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class Gallary
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public string? PhotoPath { get; set; }

    public virtual Tour? Tour { get; set; }
}
