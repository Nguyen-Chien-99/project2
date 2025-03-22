using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilityManagement.Models;

namespace FacilityManagement.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly FacilityManagementContext _context;

        public MaintenancesController(FacilityManagementContext context)
        {
            _context = context;
        }

        // GET: Maintenances
        public async Task<IActionResult> Index()
        {
            var facilityManagementContext = _context.Maintenances.Include(m => m.Equipment);
            return View(await facilityManagementContext.ToListAsync());
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances
                .Include(m => m.Equipment)
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // GET: Maintenances/Create
        public IActionResult Create(int? staffId)
        {
            if (staffId == null)
            {
                return NotFound();
            }

            // Lấy danh sách phòng của nhân viên theo staffId
            var rooms = _context.Rooms
                .Where(r => r.Staff.Any(s => s.StaffId == staffId))  // Lọc phòng theo nhân viên
                .Include(r => r.Equipment) // Bao gồm thiết bị trong phòng
                .ToList();

            // Lấy danh sách thiết bị trong các phòng mà nhân viên quản lý
            var equipments = rooms.SelectMany(r => r.Equipment).ToList();

            // Chuyển thiết bị thành SelectList để hiển thị trong dropdown
            ViewData["EquipmentId"] = new SelectList(equipments, "EquipmentId", "EquipmentName");

            return View();
        }


        // POST: Maintenances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaintenanceId,EquipmentId,MaintenanceDate,Description,Status")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                var equipment = await _context.Equipment.FindAsync(maintenance.EquipmentId);
                if (equipment != null)
                {
                    equipment.Status = "Đang bảo trì"; // Cập nhật trạng thái thiết bị
                    _context.Update(equipment); // Cập nhật thiết bị
                }
                _context.Add(maintenance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", maintenance.EquipmentId);
            return View(maintenance);
        }

        // GET: Maintenances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
            {
                return NotFound();
            }
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", maintenance.EquipmentId);
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaintenanceId,EquipmentId,MaintenanceDate,Description,Status")] Maintenance maintenance)
        {
            if (id != maintenance.MaintenanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceExists(maintenance.MaintenanceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", maintenance.EquipmentId);
            return View(maintenance);
        }

        // GET: Maintenances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances
                .Include(m => m.Equipment)
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceExists(int id)
        {
            return _context.Maintenances.Any(e => e.MaintenanceId == id);
        }
    }
}
