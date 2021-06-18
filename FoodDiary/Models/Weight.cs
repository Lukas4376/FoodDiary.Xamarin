using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDiary.Models
{
    public class Weight
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public float Mass { get; set; }
    }
}
