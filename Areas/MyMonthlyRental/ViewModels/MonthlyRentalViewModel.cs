using MyGoParking.PartialClass;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyGoParking.PartialClass;


namespace MyGoParking.Areas.MyMonthlyRental.ViewModels
{
    public class MonthlyRentalViewModel
    {
        //創建表單顯示內容
        [Required(ErrorMessage ="請選擇停車場")]
        [Display(Name ="停車場名稱")]
        public string LotName { get; set; }
        
        [Required(ErrorMessage = "請選擇車位號碼")]
        [Display(Name = "車位號碼")]
        public string SlotNumber { get; set; }

        [Required(ErrorMessage = "請輸入車牌號碼")]
        [CarExists]
        [Display(Name = "車牌號碼")]
        public string LicensePlate { get; set; }

        [Required(ErrorMessage = "請選擇開始日期")]
        [Display(Name = "開始日期")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "結束日期")]
        [Display(Name = "結束日期")]
        public DateTime EndDate { get; set; }

        //
        public int MonId { get; set; }

        [Display(Name = "月租費用")]
        public int Amount { get; set; }
        [Display(Name = "付款日期")]
        public DateTime? PaymentTime { get; set; }
        [Display(Name = "是否已繳費")]
        public bool PaymentStatus { get; set; }
        [Display(Name = "是否已通知")]
        public bool NotificationStatus { get; set; }


    }
}
