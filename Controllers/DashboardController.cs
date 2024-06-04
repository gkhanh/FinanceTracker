using System.Globalization;
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
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            // ViewBag.TotalIncome = TotalIncome.ToString("C0");
            ViewBag.TotalExpense = String.Format(culture, "{0:C2}", TotalIncome);

            //Total expense calculation
            int TotalExpense = SelectedTransactions.Where(i => i.Category.MoneyType == "Expense").Sum(j => j.Amount);
            // ViewBag.TotalExpense = TotalExpense.ToString("C0");
            ViewBag.TotalExpense = String.Format(culture, "{0:C2}", TotalExpense);

            //Balance calculation
            int Balance = TotalIncome - TotalExpense;
            
            culture.NumberFormat.CurrencyNegativePattern = 1;
            //ViewBag.Balance = Balance.ToString("C0");
            ViewBag.Balance = String.Format(culture, "{0:C2}", Balance);

            ViewBag.DoughnutChartData = SelectedTransactions.Where(i => i.Category.MoneyType == "Expense")
                .GroupBy(j => j.Category.CategoryId).Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.CategoryIcon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                }).OrderByDescending(l => l.amount).ToList();

            ViewBag.RecentTransactions = await _context.Transactions.Include(i => i.Category)
                .OrderByDescending(j => j.Date).Take(5).ToListAsync();

            //Spline Chart - Income vs Expense
            //Income
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Category.MoneyType == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.MoneyType == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();
            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();
            ViewBag.SplineChartData = from day in Last7Days
                join income in IncomeSummary on day equals income.day into dayIncomeJoined
                from income in dayIncomeJoined.DefaultIfEmpty()
                join expense in ExpenseSummary on day equals expense.day into expenseJoined
                from expense in expenseJoined.DefaultIfEmpty()
                select new
                {
                    day = day,
                    income = income == null ? 0 : income.income,
                    expense = expense == null ? 0 : expense.expense,
                };
            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();
            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}