using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models;

public partial class Destination
{
    public int Id { get; set; }

    public string? Country { get; set; }

    public string? Destinationcategory { get; set; }

    public string? PhotoCategory { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();

	[NotMapped]
	public IFormFile? PhotoFile { get; set; }
}
