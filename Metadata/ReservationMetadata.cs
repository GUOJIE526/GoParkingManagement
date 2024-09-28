using MyGoParking.Areas.MyMonthlyRental;
using MyGoParking.Models;
using MyGoParking.PartialClass;
using System.ComponentModel.DataAnnotations;

namespace MyGoParking.Metadata
{
    internal class ReservationMetadata
    {
        [Display(Name = "停車場")]
        public int? LotId { get; set; }
        //[Required(ErrorMessage = "請輸入車牌")]
        [Display(Name = "車牌號碼")]
        public int? CarId { get; set; }
        [Required(ErrorMessage = "請輸入預訂時間")]
        [Display(Name = "預訂時間")]
        public DateTime? ReservationTime { get; set; }
        [Display(Name = "逾期時間")]
        public DateTime? ValidUntil { get; set; }
        [Display(Name = "訂金付款狀態")]
        public bool? DepositStatus { get; set; }
        [Display(Name = "超時")]
        public bool? IsOverdue { get; set; }
        [Display(Name = "取消")]
        public bool? IsCanceled { get; set; }
        [Display(Name = "通知狀態")]
        public bool? NotificationStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "訂金金額")]
        public int? Amount { get; set; }
        [Display(Name = "訂金退款狀態")]
        public bool? IsRefoundDeposit { get; set; }
        [Display(Name = "訂單完成")]
        public bool? IsFinish { get; set; }
        [Display(Name = "車牌號碼")]
        public virtual Car? Car { get; set; }
        [Display(Name = "停車場")]
        public virtual ParkingLot? Lot { get; set; }

    }
}
