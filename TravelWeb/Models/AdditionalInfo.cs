using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class AdditionalInfo
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public string? Title { get; set; }

    public string? SubTitle { get; set; }

    public string? Description { get; set; }

    public virtual Tour? Tour { get; set; }
}
