using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class Goal
{
    [Key]
    public int GoalID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public long Target { get; set; }

    [DefaultValue(0)]
    public long CurrentAmount { get; set; }

    public string Reminder { get; set; }

    public DateTime? Deadline { get; set; }

    public string Status { get; set; }

    public string? Resources { get; set; }

    [Required]
    public int UserID { get; set; }

    [Required]
    public string CategoryName { get; set; }

    //Relationship
    public virtual User? User { get; set; }
    public virtual List<GoalHistory> GoalHistories { get; set; } = new List<GoalHistory>();
    public virtual GoalCategory? GoalCategory { get; set; }

    public Goal() { }

    public Goal(string name, long target, long goalAmount, string? resources = null)
    {
        Name = name;
        Target = target;
        CurrentAmount = goalAmount;
        Resources = resources;
    }
}