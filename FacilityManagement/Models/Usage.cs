using System;
using System.Collections.Generic;

namespace FacilityManagement.Models;

public partial class Usage
{
    public int UsageId { get; set; }

    public int? RoomId { get; set; }

    public string? UsedBy { get; set; }

    public DateOnly UsageDate { get; set; }

    public string? Purpose { get; set; }

    public virtual Room? Room { get; set; }
}
