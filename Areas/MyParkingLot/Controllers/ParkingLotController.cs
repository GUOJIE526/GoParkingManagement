using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;

namespace MyGoParking.Areas.MyParkingLot.Controllers
{
    [Authorize]
    [Area("MyParkingLot")]
    public class ParkingLotController : Controller
    {
        private readonly MyGoParkingContext _context;

        public ParkingLotController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyParkingLot/ParkingLot
        public async Task<IActionResult> Index()
        {

            return View(_context.ParkingLot);
        }

        public async Task<IActionResult> IndexJson()
        {
            var ParkLot = _context.ParkingLot.Select(p => new
            {
                lotId = p.LotId,
                lotName = p.LotName,
                lotAddress = p.LotAddress,
                longitude = p.Longitude ?? 0,
                latitude = p.Latitude ?? 0,
                qty = p.Qty,
                etcqty = p.Etcqty,
                monqty = p.Monqty,
                isResStatus = p.IsResStatus ? "可預訂" : "不可預訂",
                isMonStatus = p.IsMonStatus ? "可月租" : "不可月租",
                contract = p.Contract,
            });
            return Json(ParkLot);
        }

        private async Task SaveUploadImage(ParkingLot parkingLot, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    string Name = file.Name;
                    string FileName = file.FileName;
                    await file.CopyToAsync(memoryStream);
                    parkingLot.Contract = file.FileName;
                    Console.WriteLine(Name + FileName);
                    var filename = parkingLot.Contract;
                    var filePath = $"wwwroot\\Contract\\{filename}.png";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
        }


        // GET: MyParkingLot/ParkingLot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLot = await _context.ParkingLot

                .FirstOrDefaultAsync(m => m.LotId == id);
            if (parkingLot == null)
            {
                return NotFound();
            }

            return View(parkingLot);
        }

        // GET: MyParkingLot/ParkingLot/Create

        public IActionResult CreatePartial()
        {
            ViewData["IsRes"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可預約", Value = "True"},
                new SelectListItem { Text = "不可預約", Value = "False"}
            }, "Value", "Text");
            ViewData["IsMon"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可月租", Value = "True"},
                new SelectListItem { Text = "不可月租", Value = "False"}
            }, "Value", "Text");

            return PartialView("_CreatePartial");
        }

        public IActionResult Create()
        {
            ViewData["IsRes"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可預約", Value = "True"},
                new SelectListItem { Text = "不可預約", Value = "False"}
            }, "Value", "Text");
            ViewData["IsMon"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可月租", Value = "True"},
                new SelectListItem { Text = "不可月租", Value = "False"}
            }, "Value", "Text");
            return View();
        }

        // POST: MyParkingLot/ParkingLot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LotId,LotName,LotAddress,Longitude,Latitude,Qty,Etcqty,Monqty,IsResStatus,IsMonStatus,Contract")] ParkingLot parkingLot, IFormFile ContractFile)
        {
            ViewData["IsRes"] = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "可預約", Value = "True"},
                    new SelectListItem { Text = "不可預約", Value = "False"}
                }, "Value", "Text", parkingLot.IsResStatus);
            ViewData["IsMon"] = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "可月租", Value = "True"},
                    new SelectListItem { Text = "不可月租", Value = "False"}
                }, "Value", "Text", parkingLot.IsMonStatus);



            if (ModelState.IsValid)
            {

                if (ContractFile != null && ContractFile.Length > 0) //其實這個if是多的 因為asp會做驗證 你上面IFormFile如果沒? 那他沒傳就會驗證失敗
                {
                    // 獲取文件名
                    var fileName = Path.GetFileName(ContractFile.FileName);

                    // 將文件名存入 ParkingLot 的 Contract 欄位
                    parkingLot.Contract = fileName;

                    // 選擇一個位置保存文件，例如 "wwwroot/uploads"
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Contract", fileName);

                    // 保存文件到伺服器
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ContractFile.CopyToAsync(stream);
                    }
                }
                
                _context.Add(parkingLot);
                await _context.SaveChangesAsync(); //先保存停車場資料獲取ID
                //新增停車場車位
                for (int i = 1; i <= parkingLot.Monqty; i++)
                {
                    var parkingSlot = new ParkingSlot
                    {
                        LotId = parkingLot.LotId,
                        SlotNumber = $"SLOT-{i}",
                        IsRented = false
                    };
                    _context.Add(parkingSlot);

                }

                await _context.SaveChangesAsync(); //儲存新增停車位資料

                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
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

            var parkingLot = await _context.ParkingLot.FindAsync(id);
            if (parkingLot == null)
            {
                return NotFound();
            }
            ViewData["IsRes"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可預約", Value = "True"},
                new SelectListItem { Text = "不可預約", Value = "False"}
            }, "Value", "Text");
            ViewData["IsMon"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可月租", Value = "True"},
                new SelectListItem { Text = "不可月租", Value = "False"}
            }, "Value", "Text");

            return PartialView("_EditPartial", parkingLot);
        }

        // GET: MyParkingLot/ParkingLot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLot = await _context.ParkingLot.FindAsync(id);
            if (parkingLot == null)
            {
                return NotFound();
            }
            ViewData["IsRes"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可預約", Value = "True"},
                new SelectListItem { Text = "不可預約", Value = "False"}
            }, "Value", "Text");
            ViewData["IsMon"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可月租", Value = "True"},
                new SelectListItem { Text = "不可月租", Value = "False"}
            }, "Value", "Text");
            return View(parkingLot);
        }

        // POST: MyParkingLot/ParkingLot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LotId,LotName,LotAddress,Longitude,Latitude,Qty,Etcqty,Monqty,IsResStatus,IsMonStatus,Contract")] ParkingLot parkingLot, IFormFile ContractFile)
        {
            ViewData["IsRes"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可預約", Value = "True"},
                new SelectListItem { Text = "不可預約", Value = "False"}
            }, "Value", "Text", parkingLot.IsResStatus);
            ViewData["IsMon"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "可月租", Value = "True"},
                new SelectListItem { Text = "不可月租", Value = "False"}
            }, "Value", "Text", parkingLot.IsMonStatus);

            //if (id != parkingLot.LotId)
            //{
            //    return NotFound();
            //}

            //if (ContractFile != null)
            //{
            //    SaveUploadImage(parkingLot, ContractFile);
            //}
            //else
            //{

            //}

            if (ModelState.IsValid)
            {

                // 檢查是否上傳了新檔案
                if (ContractFile != null && ContractFile.Length > 0)
                {
                    // 保存上傳文件
                    var fileName = Path.GetFileName(ContractFile.FileName);
                    parkingLot.Contract = fileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Contract", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ContractFile.CopyToAsync(stream);
                    }
                }

                //處理月租車位(如果月租車位有變動)
                var originSlotCount = _context.ParkingSlot.Where(n => n.LotId == parkingLot.LotId).Count();
                if(parkingLot.Monqty > originSlotCount)
                {
                    int plus_slot = (int)(parkingLot.Monqty - originSlotCount);
                    for (int i = 1 ; i <= plus_slot; i++)
                    {
                        var parkingSlot = new ParkingSlot
                        {
                            LotId = parkingLot.LotId,
                            SlotNumber = $"SLOT-{originSlotCount + 1}",
                            IsRented = false
                        };
                        originSlotCount++;
                        _context.Add(parkingSlot);
                    }
                }else if(parkingLot.Monqty < originSlotCount)
                {
                    int remove_slot = (int)(originSlotCount -parkingLot.Monqty);

                    var updateLot = _context.ParkingSlot
                        .Where(n => n.LotId == parkingLot.LotId)
                        .OrderByDescending(slot => slot.SlotNumber)
                        .Take(remove_slot)
                        .ToList();
                    _context.RemoveRange(updateLot);

                }



                try
                {
                    _context.Update(parkingLot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingLotExists(parkingLot.LotId))
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


                //return View(parkingLot);
            }
        }

            // GET: MyParkingLot/ParkingLot/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var parkingLot = await _context.ParkingLot

                    .FirstOrDefaultAsync(m => m.LotId == id);
                if (parkingLot == null)
                {
                    return NotFound();
                }

                return View(parkingLot);
            }

            // POST: MyParkingLot/ParkingLot/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var parkingLot = await _context.ParkingLot.FindAsync(id);
                if (parkingLot != null)
                {
                    _context.ParkingLot.Remove(parkingLot);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool ParkingLotExists(int id)
            {
                return _context.ParkingLot.Any(e => e.LotId == id);
            }
        }
    }

