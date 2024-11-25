using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class GoalCategory
{
    [Key]
    public string Name { get; set; }
    public string? Description { get; set; }
    public virtual List<Goal> Goals { get; set; }
    public GoalCategory() { }
    public GoalCategory(string namegoal, string description) {
        Name = namegoal;
        Description = description;
    }
}