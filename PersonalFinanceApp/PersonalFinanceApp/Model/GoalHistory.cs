using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class GoalHistory
{
    public int GoalID { get; set; }
    public DateTime TimeAdded { get; set; }

    [Required]
    public long Amount { get; set; }

    //Relationship
    public Goal Goal { get; set; }
}