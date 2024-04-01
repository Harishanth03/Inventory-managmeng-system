using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectUC
{
    internal class Fuel_Calculation_OOP
    {
        public string voucher_number {  get; set; }
        public string vehicle_number { get; set; }
        public string fuel_type { get; set; }
        public int per_liter_cost { get; set; }
        public int total_liter {  get; set; }
        public string DateTime { get; set; }
        public int total_cost { get; set; }

        //Calculation function
        public int fuel_calcilation  (int per_liter_cost ,int total_liters)
        {
            total_cost = per_liter_cost * total_liters;
            return total_cost;
        }
            
    }
}
