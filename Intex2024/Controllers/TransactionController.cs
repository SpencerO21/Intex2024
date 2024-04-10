using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intex2024.Controllers
{
    public class TransactionController : Controller
    {
        private ITransactionRepository repository;
        private Cart cart;

        public TransactionController(ITransactionRepository repoService,
                Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }
        public ViewResult Checkout() => View(new Transaction());

        [HttpPost]
        public IActionResult Checkout(Transaction transaction)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("",
                    "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                transaction.Lines = cart.Lines.ToArray();
                repository.SaveOrder(transaction);
                cart.Clear();
                return RedirectToPage("/Confirmation",
                    new { transactionId = transaction.TransactionId });
            }
            else
            {
                return View();
            }
        }
    }
}
