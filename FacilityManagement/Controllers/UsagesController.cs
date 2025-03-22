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
    public class UsagesController : Controller
    {
        private readonly FacilityManagementContext _context;

        public UsagesController(FacilityManagementContext context)
        {
            _context = context;
        }

        // GET: Usages
        public async Task<IActionResult> Index()
        {
            var email = HttpContext.Session.GetString("CustomerLogin") ?? "";
            if (email == "")
            {
                return RedirectToAction("Index","Home");
            }
            var staff = await _context.Staff.Where(s => s.Email == email).FirstOrDefaultAsync();
            ViewBag.Name = staff.StaffName??null;
            var facilityManagementContext = _context.Usages.Include(u => u.Room);
            return View(await facilityManagementContext.ToListAsync());
        }

        // GET: Usages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages
                .Include(u => u.Room)
                .FirstOrDefaultAsync(m => m.UsageId == id);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        // GET: Usages/Create
        public IActionResult Create()
        {
            // Lấy CustomerId từ session
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Kiểm tra nếu CustomerId không có trong session
            if (customerId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Index", "LoginC");
            }

            // Lấy tên khách hàng từ session
            var staffName = HttpContext.Session.GetString("CustomerLogin");

            // Gán tên khách hàng vào ViewBag để hiển thị trong view
            ViewBag.Name = staffName ?? "Khách hàng chưa đăng nhập"; // Cung cấp thông báo mặc định nếu staffName null

            // Tạo một đối tượng Usage mới và gán giá trị cho UsedBy
            var usage = new Usage
            {
                UsedBy = staffName // Tự động điền tên khách hàng vào UsedBy
            };

            // Tạo danh sách các phòng và gán vào ViewBag
            // Nếu có giá trị RoomId (ví dụ như từ người dùng đã chọn trước), truyền vào để làm giá trị mặc định
            var selectedRoomId = usage.RoomId ?? 0; // Thay thế 0 bằng RoomId mặc định nếu có
                                                    // Lấy danh sách phòng và tạo SelectList với cả RoomName và RoomType
            ViewBag.RoomId = new SelectList(_context.Rooms
                                    .Select(r => new
                                    {
                                        r.RoomId,
                                        DisplayText = r.RoomName + " - " + r.RoomType // Kết hợp RoomName và RoomType
                                    }),
                                    "RoomId",
                                    "DisplayText",
                                    selectedRoomId);


            // Trả về view với đối tượng usage
            return View(usage);
        }


        // POST: Usages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsageId,RoomId,UsedBy,UsageDate,Purpose")] Usage usage)
        {
            // Gán giá trị UsedBy từ session
            var staffName = HttpContext.Session.GetString("CustomerLogin");
            if (!string.IsNullOrEmpty(staffName))
            {
                usage.UsedBy = staffName; // Gán giá trị cho UsedBy từ session
            }

            // Kiểm tra tính hợp lệ của mô hình
            if (ModelState.IsValid)
            {
                // Thêm đối tượng Usage vào context và lưu vào cơ sở dữ liệu
                _context.Add(usage);
                await _context.SaveChangesAsync();

                // Chuyển hướng về trang Index sau khi tạo mới thành công
                return RedirectToAction(nameof(Index));
            }

            // Cập nhật ViewData để cung cấp danh sách phòng cho dropdown
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName", usage.RoomId);

            // Trả về view với đối tượng Usage nếu có lỗi validation
            return View(usage);
        }


        // GET: Usages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages.FindAsync(id);
            if (usage == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", usage.RoomId);
            return View(usage);
        }

        // POST: Usages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsageId,RoomId,UsedBy,UsageDate,Purpose")] Usage usage)
        {
            if (id != usage.UsageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsageExists(usage.UsageId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", usage.RoomId);
            return View(usage);
        }

        // GET: Usages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages
                .Include(u => u.Room)
                .FirstOrDefaultAsync(m => m.UsageId == id);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        // POST: Usages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usage = await _context.Usages.FindAsync(id);
            if (usage != null)
            {
                _context.Usages.Remove(usage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsageExists(int id)
        {
            return _context.Usages.Any(e => e.UsageId == id);
        }
    }
}
