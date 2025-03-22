using System;
using System.Collections.Generic;

namespace FacilityManagement.Models;

public partial class Campus
{
    public int CampusId { get; set; }

    public string CampusName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
}
