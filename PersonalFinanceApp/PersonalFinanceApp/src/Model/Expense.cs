﻿using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class Expense
{
    [Key] public int ExpenseID { get; set; }

    [Required] [Range(1, 1000000000000)] public long Amount { get; set; }

    [Required] [MinLength(1)] public string Name { get; set; }

    public string? Description { get; set; }

    [Required] public DateOnly Date { get; set; }

    [Required] public DateTime TimeAdded { get; set; }

    [Required] public bool Deleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    //Relationship
    [Required] public int CategoryID { get; set; }
    public virtual Category? Category { get; set; }

    public int? RecurringExpenseID { get; set; } = null;
    public virtual RecurringExpense? RecurringExpense { get; set; }

    [Required] [Range(1, 12)] public int ExBMonth { get; set; }
    [Required] [Range(1, 3000)] public int ExBYear { get; set; }
    [Required] public int UserID { get; set; }
    public virtual ExpensesBook? ExpensesBook { get; set; }

    public Expense() { }

    public Expense(long amount, string name, DateOnly date, int categoryId, int exBMonth, int exBYear, int userId, string? description = null)
    {
        Amount = amount;
        Name = name;
        Date = date;
        CategoryID = categoryId;
        ExBMonth = exBMonth;
        ExBYear = exBYear;
        UserID = userId;
        Description = description;
        TimeAdded = DateTime.Now;
        Deleted = false;
    }
    public Expense(long amount, string name, DateOnly date, int categoryId, int exBMonth, int exBYear, int userId, int recexpID, string? description = null) {
        Amount = amount;
        Name = name;
        Date = date;
        CategoryID = categoryId;
        ExBMonth = exBMonth;
        ExBYear = exBYear;
        UserID = userId;
        Description = description;
        TimeAdded = DateTime.Now;
        Deleted = false;
        RecurringExpenseID = recexpID;
    }

    public Expense(long amount, string name, DateOnly date, Category ca, ExpensesBook exB, string? description = null)
    {
        Amount = amount;
        Name = name;
        Date = date;
        CategoryID = ca.CategoryID;
        ExBMonth = exB.Month;
        ExBYear = exB.Year;
        UserID = exB.UserID;
        Description = description;
        TimeAdded = DateTime.Now;
        Deleted = false;
    }

}