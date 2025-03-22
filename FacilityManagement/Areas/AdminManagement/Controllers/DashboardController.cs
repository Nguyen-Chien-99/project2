using FacilityManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilityManagement.Areas.AdminManagement.Controllers {

    public class DashboardController : BaseController
    {
        private readonly FacilityManagementContext _context;

        public DashboardController(FacilityManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalEquipment = _context.Equipment.Count();
            var brokenEquipment = _context.Equipment.Count(e => e.Status == "Hỏng");
            var availableEquipment = _context.Equipment.Count(e => e.Status == "Sẵn sàng");
            var underRepairEquipment = _context.Equipment.Count(e => e.Status == "Đang sửa chữa");

            var completedMaintenance = _context.Maintenances.Count(m => m.Status == "Hoàn thành");
            var pendingMaintenance = _context.Maintenances.Count(m => m.Status == "Chưa giải quyết");

            var statistics = new
            {
                TotalEquipment = totalEquipment,
                BrokenEquipment = brokenEquipment,
                AvailableEquipment = availableEquipment,
                UnderRepairEquipment = underRepairEquipment,
                CompletedMaintenance = completedMaintenance,
                PendingMaintenance = pendingMaintenance
            };

            return View(statistics);
        }
    }

}
