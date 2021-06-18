using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDiary.Models
{
    public class Payment
    {
        public string Token { get; set; }
        public decimal Amount { get; set; }
    }
}
