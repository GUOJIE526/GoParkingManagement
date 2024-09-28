using MyGoParking.Models;

namespace MyGoParking.Areas.MyParkingSlot.ViewModels
{
    public class ParkingSlotManageViewModel
    {
        public IEnumerable<ParkingSlot> ParkingSlots { get; set; }  // 用於存取月租"車位資訊"資料
        public IEnumerable<MonApplyList> MonApplyList { get; set; }  // 用于存取"月租申请"的資料

        public IEnumerable<ParkingLot> ParkingLots { get; set; }  // 所有停車場的資料(下拉式選單用)

        public string lotName { get; set; }  //停車場名字
        public string lotAddress { get; set; } //停車場地址
        public int TotalLots { get; set; }   //月租車位數 
        public int Rentedlots { get; set; }  //月租車位已出租數
        public double RentalRate  //出租率
        {
            get
            {
                // 防止除以零的情况
                if (TotalLots == 0)
                    return 0;

                return Math.Round((double)Rentedlots / TotalLots * 100); // 返回百分比形式
            }
        }
        public int WaitingApplicants { get; set; }  //等候人數
    }
}
