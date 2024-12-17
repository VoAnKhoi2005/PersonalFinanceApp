using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class GoalCategory
{
    [Key]
    public string Name { get; set; }

    [Required]
    public int UserID { get; set; }
    public virtual User? User { get; set; }

    public virtual List<Goal> Goals { get; set; }
    public GoalCategory() { }
    public GoalCategory(string namegoal) {
        Name = namegoal;
    }
}