using System;
using System.Collections.Generic;

namespace FacilityManagement.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public int? BuildingId { get; set; }

    public string RoomType { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Description { get; set; }

    public virtual Building? Building { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<Usage> Usages { get; set; } = new List<Usage>();
}
