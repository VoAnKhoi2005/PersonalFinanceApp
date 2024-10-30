using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [MaxLength(40)]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    [Range(0, 1000000000000000)]
    public long Saving { get; set; }

    [Range(0, 1000000000000000)]
    public long DefaultBudget { get; set; }

    [MaxLength(256)]
    public string? Resources { get; set; }

    //Relationship
    public virtual List<ExpensesBook> ExpensesBooks { get; set; } = new List<ExpensesBook>();
    public virtual List<Goal> Goals { get; set; } = new List<Goal>();

    public User() { }

    public User(string username, string password, string email, string? phoneNumber = null, long saving = 0, long defaultBudget = 0, string? resources = null)
    {
        Username = username;
        Password = password;
        Email = email;
        PhoneNumber = phoneNumber;
        Saving = saving;
        DefaultBudget = defaultBudget;
        Resources = resources;
    }
}