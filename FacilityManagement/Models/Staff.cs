using System;
using System.Collections.Generic;

namespace FacilityManagement.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string StaffName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? AssignedRoomId { get; set; }

    public virtual Room? AssignedRoom { get; set; }
}
