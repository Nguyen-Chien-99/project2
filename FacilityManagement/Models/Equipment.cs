using System;
using System.Collections.Generic;

namespace FacilityManagement.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string? Image { get; set; }

    public string EquipmentName { get; set; } = null!;

    public string EquipmentType { get; set; } = null!;

    public int? RoomId { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    public virtual Room? Room { get; set; }
}
