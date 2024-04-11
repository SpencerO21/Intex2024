using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using Intex2024.Infrastructure;
using Intex2024.Models;

namespace Intex2024.Pages
{
    public class CheckoutModel : PageModel
    {
        private IStoreRepository _repo;
        public CheckoutModel(IStoreRepository temp)
        {
            _repo = temp;
        }

        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product prod = _repo.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (prod != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                //Cart.AddItem(prod, 1);
                HttpContext.Session.SetJson("cart", Cart);

            }

            return RedirectToPage(new { returnUrl = returnUrl });

        }
    }
}
