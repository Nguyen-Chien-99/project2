using System;
using System.Collections.Generic;

namespace FacilityManagement.Models;

public partial class Maintenance
{
    public int MaintenanceId { get; set; }

    public int? EquipmentId { get; set; }

    public DateOnly MaintenanceDate { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual Equipment? Equipment { get; set; }
}
