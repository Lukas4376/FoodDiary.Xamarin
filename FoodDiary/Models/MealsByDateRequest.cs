using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDiary.Models
{
    public class MealsByDateRequest
    {
        public int userId { get; set; }
        public string date { get; set; }
    }
}
