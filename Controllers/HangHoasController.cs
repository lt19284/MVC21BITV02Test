using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webapplication.Data;

namespace Webapplication.Controllers
{
    public class HangHoasController : Controller
    {
        private readonly MyeStoreContext _context;
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", "HangHoa");

        public HangHoasController(MyeStoreContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var myeStoreContext = _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation);
            return View(await myeStoreContext.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

      
        public IActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(_context.Loais, "MaLoai", "MaLoai");
            ViewBag.MaNcc = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHh,TenHh,TenAlias,MaLoai,MoTaDonVi,DonGia,NgaySx,GiamGia,SoLanXem,MoTa,MaNcc")] HangHoa hangHoa, IFormFile? hinhFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (hinhFile != null && hinhFile.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(hinhFile.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(hinhFile.FileName);
                        var filePath = Path.Combine(_uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await hinhFile.CopyToAsync(fileStream);
                        }

                        hangHoa.Hinh = fileName;
                    }
                    else
                    {
                        hangHoa.Hinh = string.Empty; 
                    }

                    _context.Add(hangHoa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Hàng hóa đã được tạo thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                }
            }

            ViewBag.MaLoai = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewBag.MaNcc = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // GET: HangHoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            ViewBag.MaLoai = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewBag.MaNcc = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHh,TenHh,TenAlias,MaLoai,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa,MaNcc")] HangHoa hangHoa, IFormFile? hinhFile)
        {
            if (id != hangHoa.MaHh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hinhFile != null && hinhFile.Length > 0)
                    {
                        var oldFilePath = Path.Combine(_uploadsFolder, hangHoa.Hinh);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        var fileName = Path.GetFileNameWithoutExtension(hinhFile.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(hinhFile.FileName);
                        var filePath = Path.Combine(_uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await hinhFile.CopyToAsync(fileStream);
                        }

                        hangHoa.Hinh = fileName;
                    }

                    _context.Update(hangHoa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Hàng hóa đã được cập nhật thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangHoaExists(hangHoa.MaHh))
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

            ViewBag.MaLoai = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewBag.MaNcc = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa != null)
            {
                var filePath = Path.Combine(_uploadsFolder, hangHoa.Hinh);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.HangHoas.Remove(hangHoa);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Hàng hóa đã được xóa thành công.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HangHoaExists(int id)
        {
            return _context.HangHoas.Any(e => e.MaHh == id);
        }
    }
}
