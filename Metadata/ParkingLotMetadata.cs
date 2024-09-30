using MyGoParking.Models;
using System.ComponentModel.DataAnnotations;

namespace MyGoParking.Metadata
{
    internal class ParkingLotMetadata
    {
        [Required(ErrorMessage = "請選擇")]
        [Display(Name ="停車場ID")]
        public int LotId { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "停車場名稱")]
        public string? LotName { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "停車場地址")]
        public string? LotAddress { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "車位數量")]
        public int? Qty { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "電動車位數量")]
        public int? Etcqty { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "月租車位數量")]
        public int? Monqty { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "可否預約")]
        public bool IsResStatus { get; set; }
        [Required(ErrorMessage = "請選擇")]
        [Display(Name = "可否月租")]
        public bool IsMonStatus { get; set; }
        //[Required(ErrorMessage = "請選擇")]
        [Display(Name = "停車場合約")]
        public string? Contract { get; set; }

        public virtual ICollection<EntryExitManagement> EntryExitManagement { get; set; } = new List<EntryExitManagement>();

        public virtual ICollection<MonApplyList> MonApplyList { get; set; } = new List<MonApplyList>();

        public virtual ICollection<MonthlyRental> MonthlyRental { get; set; } = new List<MonthlyRental>();

        public virtual ICollection<ParkingSlot> ParkingSlot { get; set; } = new List<ParkingSlot>();

        public virtual ICollection<Reservation> Reservation { get; set; } = new List<Reservation>();
    }
}