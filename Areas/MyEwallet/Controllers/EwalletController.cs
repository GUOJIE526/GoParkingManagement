using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;

namespace MyGoParking.Areas.MyEwallet.Controllers
{
    [Authorize]
    [Area("MyEwallet")]
    public class EwalletController : Controller
    {
        private readonly MyGoParkingContext _context;

        public EwalletController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyEwallet/Ewallet
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.Ewallet.Include(e => e.User);
            return View(await myGoParkingContext.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var myGoParkingContext = _context.Ewallet.Include(e => e.User)
                .Select(e => new
                {
                    walletId = e.WalletId,
                    userId = e.User.Username,
                    balance = e.Balance.ToString("C0", CultureInfo.CurrentCulture),
                    updateTime = e.UpdatedTime.HasValue? e.UpdatedTime.Value.ToString("yyyy/M/d tt hh:mm:ss") :null,
                });
            return Json(myGoParkingContext);
        }

        // GET: MyEwallet/Ewallet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ewallet = await _context.Ewallet
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (ewallet == null)
            {
                return NotFound();
            }

            return View(ewallet);
        }

        public IActionResult CreatePartial()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return PartialView("_CreatePartial");
        }

        // GET: MyEwallet/Ewallet/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return View();
        }

        // POST: MyEwallet/Ewallet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WalletId,UserId,Balance,UpdatedTime")] Ewallet ewallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ewallet);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ewallet = _context.Ewallet.Find(id);
            if (ewallet == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", ewallet.UserId);
            return PartialView("_EditPartial", ewallet);

        }

        // GET: MyEwallet/Ewallet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ewallet = await _context.Ewallet.FindAsync(id);
            if (ewallet == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", ewallet.UserId);
            return View(ewallet);
        }

        // POST: MyEwallet/Ewallet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WalletId,UserId,Balance,UpdatedTime")] Ewallet ewallet)
        {
            if (id != ewallet.WalletId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ewallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EwalletExists(ewallet.WalletId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // GET: MyEwallet/Ewallet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ewallet = await _context.Ewallet
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (ewallet == null)
            {
                return NotFound();
            }

            return View(ewallet);
        }

        // POST: MyEwallet/Ewallet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ewallet = await _context.Ewallet.FindAsync(id);
            if (ewallet != null)
            {
                try
                {
                    _context.Ewallet.Remove(ewallet);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // 在發生錯誤時，返回錯誤信息
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "找不到此加值記錄" });
            //return RedirectToAction(nameof(Index));
        }

        private bool EwalletExists(int id)
        {
            return _context.Ewallet.Any(e => e.WalletId == id);
        }
    }
}
