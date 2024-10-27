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
    public User User { get; set; }
}