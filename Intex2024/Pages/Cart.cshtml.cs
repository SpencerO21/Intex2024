using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using Intex2024.Infrastructure;
using Intex2024.Models;

namespace Intex2024.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository _repo;

        public Cart Cart { get; set; }
        public CartModel(IStoreRepository temp, Cart cartService)
        {
            _repo = temp;
            Cart = cartService;
        }

        
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(short productId, string returnUrl)
        {
            Product? prod = _repo.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (prod != null)
            {
                Cart.AddItem(prod, 1);

            }

            return RedirectToPage(new { returnUrl = returnUrl });

        }

        public IActionResult OnPostRemove(short productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl});
        }
    }

}
