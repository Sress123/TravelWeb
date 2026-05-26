using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWeb.Models;

public partial class Tour
{
    public int Id { get; set; }

    public int? DestinationId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Duration { get; set; }

    public string? PhotoPath { get; set; }

    public string? Grade { get; set; }

    public string? BestSeasons { get; set; }

    public string? Altitude { get; set; }

    public string? Region { get; set; }

    public string? Accommodation { get; set; }

    public string? RouteMapPhotoPath { get; set; }

    public virtual ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();

    public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; } = new List<AdditionalInfo>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<DatePrice> DatePrices { get; set; } = new List<DatePrice>();

    public virtual Destination? Destination { get; set; }

    public virtual ICollection<Gallary> Gallaries { get; set; } = new List<Gallary>();

    public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

	[NotMapped]
	public IFormFile? PhotoFile { get; set; }

	[NotMapped]
	public IFormFile? RouteMapPhotoFile { get; set; }
}
