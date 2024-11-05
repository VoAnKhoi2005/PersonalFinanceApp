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
    public long GoalAmount { get; set; }

    public string? Resources { get; set; }

    //Relationship
    [Required]
    public int UserID { get; set; }
    public virtual User User { get; set; }

    public virtual List<GoalHistory> GoalHistories { get; set; } = new List<GoalHistory>();

    public Goal() { }

    public Goal(string name, long target, long goalAmount, string? resources = null)
    {
        Name = name;
        Target = target;
        GoalAmount = goalAmount;
        Resources = resources;
    }
}