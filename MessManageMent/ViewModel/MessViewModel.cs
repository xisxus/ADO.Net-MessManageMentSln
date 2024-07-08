using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessManageMent.ViewModel
{
    public class MessViewModel
    {
        public int Id { get; set; }
        public DateTime MonthCalc { get; set; }
        public string MemName { get; set; }
        public string Picture { get; set; }
        public int Rent { get; set; }
        public int InternetBill { get; set; }
        public int ElectricBill { get; set; }
        public int GasBill { get; set; }
        public int FridgeBill { get; set; }
        public int CostOfFood { get; set; }
        public int Meal { get; set; }
        public decimal MealRate { get; set; }
        public decimal ConsumeFood { get; set; }
        public decimal RestOfMoney { get; set; }
        public decimal Utility { get; set; }
        public int otherCost { get; set; }
        public int TotalMoney { get; set; }
        public int MemId { get; set; }
    }
}
