﻿using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class GoalHistory
{
    public int GoalID { get; set; }
    public DateTime TimeAdded { get; set; }

    [Required]
    public string Amount { get; set; }
    public string Current { get; set; }

    //Relationship
    public virtual Goal? Goal { get; set; }

    public GoalHistory() { }

    public GoalHistory(Goal goal, string amount)
    {
        GoalID = goal.GoalID;
        TimeAdded = DateTime.Now;
        Amount = amount;
    }
}