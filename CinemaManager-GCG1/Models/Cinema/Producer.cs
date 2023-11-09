using System;
using System.Collections.Generic;

namespace CinemaManager_GCG1.Models.Cinema;

public partial class Producer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string Email { get; set; } = null!;
}
