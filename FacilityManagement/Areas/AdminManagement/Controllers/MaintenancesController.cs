using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilityManagement.Models;

namespace FacilityManagement.Areas.AdminManagement.Controllers
{
    [Area("AdminManagement")]
    public class MaintenancesController : Controller
    {
        private readonly FacilityManagementContext _context;

        public MaintenancesController(FacilityManagementContext context)
        {
            _context = context;
        }

        // GET: AdminManagement/Maintenances
        public async Task<IActionResult> Index()
        {
            var facilityManagementContext = _context.Maintenances.Include(m => m.Equipment);
            return View(await facilityManagementContext.ToListAsync());
        }

        // GET: AdminManagement/Maintenances/Details/5
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

        // GET: AdminManagement/Maintenances/Create
        public IActionResult Create()
        {
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentName");
            return View();
        }

        // POST: AdminManagement/Maintenances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaintenanceId,EquipmentId,MaintenanceDate,Description,Status")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentId", maintenance.EquipmentId);
            return View(maintenance);
        }

        // GET: AdminManagement/Maintenances/Edit/5
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

            // Đặt danh sách các thiết bị vào ViewData
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentName", maintenance.EquipmentId);

            // Trả về View với dữ liệu Maintenance
            return View(maintenance);
        }


        // POST: AdminManagement/Maintenances/Edit/5
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
                return RedirectToAction(nameof(Index)); // Quay lại danh sách Maintenance
            }

            // Nếu model không hợp lệ, trả về View
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "EquipmentId", "EquipmentName", maintenance.EquipmentId);
            return View(maintenance);
        }


        // GET: AdminManagement/Maintenances/Delete/5
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

        // POST: AdminManagement/Maintenances/Delete/5
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
