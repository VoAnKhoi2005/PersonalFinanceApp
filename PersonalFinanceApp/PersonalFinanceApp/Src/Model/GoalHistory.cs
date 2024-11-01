using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class GoalHistory
{
    public int GoalID { get; set; }
    public DateTime TimeAdded { get; set; }

    [Required]
    public long Amount { get; set; }

    //Relationship
    public virtual Goal? Goal { get; set; }

    public GoalHistory() { }

    public GoalHistory(Goal goal, long amount)
    {
        GoalID = goal.GoalID;
        TimeAdded = DateTime.Now;
        Amount = amount;
    }
}