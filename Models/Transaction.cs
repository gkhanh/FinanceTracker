using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_Tracking_Web_Application.Models;

public class Transaction
{
    //TransactionId will be primary key for the database
    [Key]
    public int TransactionId { get; set; } // transaction ID 
    
    //Throw error
    [Range(1,int.MaxValue,ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; } // category ID 
    public Category? Category { get; set; } // specify the foreign key to link Transaction and Category table
    
    //Throw error
    [Range(1,int.MaxValue,ErrorMessage = "Amount must be greater than 0.")]
    public int Amount { get; set; } // Transaction amount
    
    [Column(TypeName = "nvarchar(100)")]
    public string? Note { get; set; } // Description of the transaction
    
    public DateTime Date { get; set; } = DateTime.Now; //date of the transaction
    
    [NotMapped]
    public string? CategoryTitleWithIcon
    {
        get
        {
            return Category == null ? "" : Category.CategoryIcon + " " + Category.Title;
        }
    }

    [NotMapped]
    public string? FormattedAmount
    {
        get
        {
            return ((Category == null || Category.MoneyType == "Expense")? "- " : "+ ") + Amount.ToString("C0");
        }
    }
}