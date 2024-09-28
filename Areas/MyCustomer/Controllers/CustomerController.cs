using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MyGoParking.Models;

namespace MyGoParking.Areas.MyCustomer.Controllers
{
    [Authorize]
    [Area("MyCustomer")]
    public class CustomerController : Controller
    {
        private readonly MyGoParkingContext _context;

        public CustomerController(MyGoParkingContext context)
        {
            _context = context;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);  // 將密碼轉換為字節
                byte[] hashBytes = sha256.ComputeHash(bytes);     // 計算SHA256雜湊值
                return Convert.ToBase64String(hashBytes);         // 將雜湊值轉換為Base64字符串以便存儲
            }
        }

        // GET: MyCustomer/Customer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        public JsonResult IndexJson()
        {
            var Cust = _context.Customer.Select(c => new
            {
                userId = c.UserId,
                username = c.Username,
                password = HashPassword(c.Password),
                email = c.Email,
                phone = c.Phone,
                blackCount = c.BlackCount,
                isBlack = c.IsBlack ? "異常" : "正常"
            });
            return Json(Cust);
        }

        // GET: MyCustomer/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: MyCustomer/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreatePartial()
        {
            ViewData["IsBlack"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "異常", Value = "True"},
                new SelectListItem {Text = "正常", Value="False"},
            }, "Value", "Text");
            return PartialView("_CreatePartial");
        }

        // POST: MyCustomer/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,Email,Phone,BlackCount,IsBlack")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Password = HashPassword(customer.Password);
                _context.Add(customer);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IsBlack"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "異常", Value = "True"},
                new SelectListItem {Text = "正常", Value="False"},
            }, "Value", "Text");
            return PartialView("_EditPartial", customer);
        }

        // GET: MyCustomer/Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: MyCustomer/Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,Email,Phone,BlackCount,IsBlack")] Customer customer)
        {
            if (id != customer.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
            return View(customer);
        }

        // GET: MyCustomer/Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: MyCustomer/Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.UserId == id);
        }
    }
}
