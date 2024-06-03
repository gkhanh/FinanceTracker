using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_Tracking_Web_Application.Models;

public class Category
{
    //CategoryId will be primary key for the database
    [Key]
    public int CategoryId { get; set; } // category ID 

    [Column(TypeName = "nvarchar(100)")] // column with 100 character max for the length of title
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = "";// category name

    [Column(TypeName = "nvarchar(100)")] // column with 100 character max for the length of Icon name
    public string CategoryIcon { get; set; } = "";// category icon display
    
    [Column(TypeName = "nvarchar(20)")] // column with 20 character max for amount of money
    public string MoneyType { get; set; } = "Expense"; // income/expense

    [NotMapped]
    public string? TitleWithIcon
    {
        get
        {
            return this.CategoryIcon + " " + this.Title;
        }
    }
}