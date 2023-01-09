using Finance_Tracking_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracking_Web_Application.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        //Constructor
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //7 days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
            List<Transaction> SelectedTransactions = await _context.Transactions.Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate).ToListAsync();
            
            //Total income calculation
            int TotalIncome = SelectedTransactions.Where(i => i.Category.MoneyType == "Income").Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");            
            
            //Total expense calculation
            int TotalExpense = SelectedTransactions.Where(i => i.Category.MoneyType == "Expense").Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");  
            
            //Balance calculation
            int Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = Balance.ToString("C0");
            
            ViewBag.DoughnutChartData = SelectedTransactions.Where(i => i.Category.MoneyType == "Expense")
                .GroupBy(j => j.Category.CategoryId).Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.CategoryIcon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                }).OrderByDescending(l => l.amount).ToList();
            
            ViewBag.RecentTransactions = await _context.Transactions.Include(i => i.Category)
                .OrderByDescending(j => j.Date).Take(5).ToListAsync();
            
            return View();
        }
    }
}
