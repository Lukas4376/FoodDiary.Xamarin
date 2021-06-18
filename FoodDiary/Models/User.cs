using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDiary.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
