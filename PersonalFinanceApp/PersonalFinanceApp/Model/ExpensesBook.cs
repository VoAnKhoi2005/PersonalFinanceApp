﻿using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model
{
    //Expenses book have 3 primary key: Month, Year and UserID
    public class ExpensesBook
    {
        [Required]
        [Range(1,12)]
        public int Month { get; set; }

        [Required]
        [Range(1,3000)]
        public int Year { get; set; }

        [Required]
        public string UserID { get; set; }

        [Range(0, 1000000000000000)]
        public long Budget { get; set; }

        [Range(0, 1000000000000000)]
        public long Spending { get; set; }

        [MaxLength(256)]
        public string? Resources { get; set; }
        
        //Relationship
        public virtual User User { get; set; }
        public virtual List<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual List<Category> Categories { get; set; } = new List<Category>();
    }
}