using Finance_Tracking_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Finance_Tracking_Web_Application.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(ILogger<PortfolioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}