using System;
using System.Collections.Generic;

namespace TravelWeb.Models;

public partial class Review
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Reviewtext { get; set; } = null!;

    public string Authorname { get; set; } = null!;

    public string Authoremail { get; set; } = null!;

    public int? Rating { get; set; }

    public bool? Isgenuine { get; set; }

    public DateTime? Createdat { get; set; }
}
