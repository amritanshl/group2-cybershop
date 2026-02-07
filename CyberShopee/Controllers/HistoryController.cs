// File: Controllers/HistoryController.cs
using CyberShopee.Models;
using CyberShopee.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberShopee.Controllers
{
    public class HistoryController : Controller
    {
        private readonly CyberShopperDbContext _context;

        public HistoryController(CyberShopperDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> History()
        {
            var customerIdStr = HttpContext.Session.GetString("CustomerId");

            // Redirect to login if the user is not logged in
            if (string.IsNullOrEmpty(customerIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            // Convert customerIdStr to int
            if (!int.TryParse(customerIdStr, out int customerId))
            {
                return RedirectToAction("Login", "Account");
            }

            var history = await (from o in _context.Orders
                                 where o.CustomerId == customerId
                                 join od in _context.OrderDetails on o.OrderId equals od.OrderId into orderGroup
                                 from od in orderGroup.DefaultIfEmpty() // This makes it a LEFT JOIN
                                 join p in _context.Products on od.ProductId equals p.ProductId into productGroup
                                 from p in productGroup.DefaultIfEmpty() // This makes it a LEFT JOIN
                                 select new OrderHistoryItem
                                 {
                                     OrderId = o.OrderId,
                                     OrderDate = o.OrderDate,
                                     ShipDate = o.ShipDate,
                                     ProductId = p != null ? p.ProductId : 0,
                                     ProductName = p != null ? p.ModelName : "Unknown Product",
                                     Quantity = od != null ? od.Quantity : 0,
                                     UnitCost = od != null ? od.UnitCost : 0
                                 }).ToListAsync();

            var viewModel = new AccountHistoryViewModel
            {
                HistoryItems = history
            };

            return View(viewModel);
        }
    }
}