using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class Service
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public string? IncludeServices { get; set; }

    public string? NotIncludeServices { get; set; }

    public virtual Tour? Tour { get; set; }
}
