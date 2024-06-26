using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex2024.Models;
using Intex2024.Models.ViewModels;

using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Elfie.Serialization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Intex2024.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IStoreRepository _repo;
    private readonly InferenceSession _session;
    private readonly IntexStoreContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(IntexStoreContext context, ILogger<HomeController> logger, IStoreRepository temp, UserManager<IdentityUser> userManager, InferenceSession session)
    {
        _context = context;
        _logger = logger;
        _repo = temp;
        _userManager = userManager;
        _session = session;
        try
        {
            _session = new InferenceSession("fraudModel.onnx");
            _logger.LogInformation("ONNX model loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading the ONNX model: {ex.Message}");
        }
    }


    public IActionResult Index(string? cat, string? color, int pageSize = 5, int pageNum = 1, int custId = 1)
    {
        var query = _repo.Products.AsQueryable();

        if (!string.IsNullOrEmpty(cat))
        {
            query = query.Where(x => x.Category1 == cat || x.Category2 == cat || x.Category3 == cat);
            ViewBag.SelectedCategory = cat;
        }

        if (!string.IsNullOrEmpty(color))
        {
            query = query.Where(x => x.PrimaryColor == color || x.SecondaryColor == color);
            ViewBag.SelectedColors = color;
        }

        var totalItems = query.Count();

        var products = query
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);
        var cust = _repo.Customers.FirstOrDefault(x => x.CustomerId == custId);
        var relatedProduct1 = _repo.Products.FirstOrDefault(x => x.ProductId == cust.RelatedItem1);
        var relatedProduct2 = _repo.Products.FirstOrDefault(x => x.ProductId == cust.RelatedItem2);
        var relatedProduct3 = _repo.Products.FirstOrDefault(x => x.ProductId == cust.RelatedItem3);

        var viewModel = new ProductListViewModel
        {
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            },
            currentCat = cat,
            currentColor = color,
            SelectedPageSize = pageSize,
            products = products,
            Customer = cust,
            Product1 = relatedProduct1,
            Product2 = relatedProduct2,
            Product3 = relatedProduct3
        };

        return View(viewModel);
    }


    [HttpGet]
    public ActionResult ProductDetails(int id)
    {
        var product = _repo.Products.FirstOrDefault(x => x.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }
        var relatedProduct1 = _repo.Products.FirstOrDefault(x => x.ProductId == product.RelatedItem1);
        var relatedProduct2 = _repo.Products.FirstOrDefault(x => x.ProductId == product.RelatedItem2);
        var relatedProduct3 = _repo.Products.FirstOrDefault(x => x.ProductId == product.RelatedItem3);

        // Initialize ProductDetailsViewModel (Should contain a Product Object as well as
        // Product objects for the related items.)

        // Instead of passing the product, pass the viewmodel

        var viewModel = new ProductDetailsViewModel
        {
            Product = product,
            RelatedProduct1 = relatedProduct1,
            RelatedProduct2 = relatedProduct2,
            RelatedProduct3 = relatedProduct3,
            LineItem = new LineItem(),
            Customer = _repo.Customers.FirstOrDefault(x => x.CustomerId == 1)
        };
        return View(viewModel);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult UserInfo()
    {
        return View(new Customer());
    }

    [HttpPost]
    public async Task<IActionResult> UserInfo(Customer cust)
    {
        var user = await _userManager.GetUserAsync(User);

        string userId = "";
        if (user != null)
        {
            userId = user.Id;
        }
        if (ModelState.IsValid)
        {
            Customer lastCustomer = _repo.Customers.OrderByDescending(t => t.CustomerId).FirstOrDefault();
            cust.CustomerId = lastCustomer.CustomerId+1;
            cust.UserId = userId;
            _repo.AddCustomer(cust);
            return RedirectToAction("Index");
        }
        else
        {
            return View("UserInfo", cust);
        }
    }

    public IActionResult Cart(Customer cust)
    {
        // Materialize the lineItems query here to avoid multiple open DataReaders.
        List<LineItem> lineItems = _repo.LineItems.Where(x => x.CartId == cust.CartId).ToList();
        List<Product> products = new List<Product>();
        Cart cart = _repo.Carts.SingleOrDefault(x => x.CartId == cust.CartId);
        int total = 0;

        foreach (LineItem lineItem in lineItems)
        {
            // Now that lineItems are fetched, there should be no open reader conflict.
            Product product = _repo.Products.FirstOrDefault(x => x.ProductId == lineItem.ProductId);
            products.Add(product);
            if (product != null) {
                total += (product.Price * lineItem.Qty);
            }
        }
        cart.Total = total;
        // Save changes to database
        _repo.UpdateCart(cart);
        var cartViewModel = new CartViewModel()
        {
            LineItems = lineItems.AsQueryable(),
            cust = cust,
            cart = cart,
            Products = products.AsQueryable()
        };

        return View(cartViewModel);
    }


    [HttpGet]
    public IActionResult Checkout(int customerID)
    {
        var checkoutViewModel = new CheckoutViewModel()
        {
            custID = customerID,
            Transaction = new Transaction()
        };
        return View("Checkout", checkoutViewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        if (ModelState.IsValid)
        {
            Transaction transaction = model.Transaction;
        
            // Calculate the transaction ID asynchronously
            var lastTransaction = await _repo.Transactions.OrderByDescending(t => t.TransactionId).FirstOrDefaultAsync();
            transaction.TransactionId = lastTransaction != null ? lastTransaction.TransactionId + 1 : 1;
        
            // Asynchronously get the customer
            Customer customer = await _repo.Customers.FirstOrDefaultAsync(x => x.CustomerId == model.custID);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }
            transaction.CustomerId = customer.CustomerId;
        
            // Asynchronously get the line items
            List<LineItem> lineItems = await _repo.GetLineItemsByCartIdAsync(customer.CartId ?? 1);
            foreach (LineItem lineItem in lineItems)
            {
                lineItem.TransactionId = model.Transaction.TransactionId;
                lineItem.CartId = null;
            }
        
            // Assume UpdateItemNoSave marks the entities as modified in the context
            await _repo.SaveChangesAsync();

            // Asynchronously add the transaction
            await _repo.AddTransactionAsync(transaction);

            return RedirectToAction("Predict", transaction);
        }
        else
        {
            return View("Checkout", model);
        }
    }



    public IActionResult Confirmation()
    {
        return View();
    }
    
    
    [HttpPost]
    public IActionResult AddItemCart(short productId, int cartId, int custId, LineItem line)
    {
        var item = line;
        item.CartId = cartId;
        item.Qty += 1;
        item.ProductId = productId;
        Customer customer = _repo.Customers.FirstOrDefault(x => x.CustomerId == custId);
        item.TransactionId = 1;
        _repo.AddItem(item);

        return RedirectToAction("Cart", customer);
    }

    // [HttpPost]
    // public IActionResult RemoveItemCart()
    // {
    //
    // }

    public IActionResult Predict(Transaction transaction)
    {
        int day_of_week_Mon = 0;
        int day_of_week_Sat = 0;
        int day_of_week_Sun = 0;
        int day_of_week_Thu = 0;
        int day_of_week_Tue = 0;
        int day_of_week_Wed = 0;
        int entry_mode_PIN = 0;
        int entry_mode_Tap = 0;
        int type_of_transaction_Online = 0;
        int type_of_transaction_POS = 0;
        int country_of_transaction_India = 0;
        int country_of_transaction_Russia = 0;
        int country_of_transaction_USA = 0;
        int country_of_transaction_United_Kingdom = 0;
        int shipping_address_India = 0;
        int shipping_address_Russia = 0;
        int shipping_address_USA = 0;
        int shipping_address_United_Kingdom = 0;
        int bank_HSBC = 0;
        int bank_Halifax = 0;
        int bank_Lloyds = 0;
        int bank_Metro = 0;
        int bank_Monzo = 0;
        int bank_RBS = 0;
        int type_of_card_Visa = 0;
    
        if (transaction.DayOfWeek == "Mon")
        {
            day_of_week_Mon = 1;
        }
        else if (transaction.DayOfWeek == "Tue")
        {
            day_of_week_Tue = 1;
        }
        else if (transaction.DayOfWeek == "Wed")
        {
            day_of_week_Wed = 1;
        }
        else if (transaction.DayOfWeek == "Thu")
        {
            day_of_week_Thu = 1;
        }
        else if (transaction.DayOfWeek == "Sat")
        {
            day_of_week_Sat = 1;
        }
        else if (transaction.DayOfWeek == "Sun")
        {
            day_of_week_Sun = 1;
        }
    
        if (transaction.EntryMode == "PIN")
        {
            entry_mode_PIN = 1;
        }
        else if (transaction.EntryMode == "Tap")
        {
            entry_mode_Tap = 1;
        }
    
        if (transaction.TypeOfTransaction == "Online")
        {
            type_of_transaction_Online = 1;
        }
        else if (transaction.TypeOfTransaction == "POS")
        {
            type_of_transaction_POS = 1;
        }
    
        if (transaction.CountryOfTransaction == "India")
        {
            country_of_transaction_India = 1;
        }
        else if (transaction.CountryOfTransaction == "Russia")
        {
            country_of_transaction_Russia = 1;
        }
        else if (transaction.CountryOfTransaction == "USA")
        {
            country_of_transaction_USA = 1;
        }
        else if (transaction.CountryOfTransaction == "United Kingdom")
        {
            country_of_transaction_United_Kingdom = 1;
        }
    
        if (transaction.ShippingAddress == "India")
        {
            shipping_address_India = 1;
        }
        else if (transaction.ShippingAddress == "Russia")
        {
            shipping_address_Russia = 1;
        }
        else if (transaction.ShippingAddress == "USA")
        {
            shipping_address_USA = 1;
        }
        else if (transaction.ShippingAddress  == "United Kingdom")
        {
            shipping_address_United_Kingdom = 1;
        }
    
        if (transaction.Bank == "HSBC")
        {
            bank_HSBC = 1;
        }
        else if (transaction.Bank == "Halifax")
        {
            bank_Halifax = 1;
        }
        else if (transaction.Bank == "Lloyds")
        {
            bank_Lloyds = 1;
        }
        else if (transaction.Bank == "Metro")
        {
            bank_Metro = 1;
        }
        else if (transaction.Bank == "Monzo")
        {
            bank_Monzo = 1;
        }
        else if (transaction.Bank == "RBS")
        {
            bank_RBS = 1;
        }
    
        if (transaction.TypeOfCard == "Visa")
        {
            type_of_card_Visa = 1;
        }
    
        var fraud_dict = new Dictionary<int, string>()
        {
            { 0, "No" },
            { 1, "Yes" }
        };
    
        try
        {
            var input = new List<float>
            {
                transaction.CustomerId, transaction.Amount, day_of_week_Mon, day_of_week_Sat,
                day_of_week_Sun, day_of_week_Thu, day_of_week_Tue,
                day_of_week_Wed, entry_mode_PIN, entry_mode_Tap,
                type_of_transaction_Online, type_of_transaction_POS,
                country_of_transaction_India, country_of_transaction_Russia,
                country_of_transaction_USA, country_of_transaction_United_Kingdom,
                shipping_address_India, shipping_address_Russia,
                shipping_address_USA, shipping_address_United_Kingdom, bank_HSBC,
                bank_Halifax, bank_Lloyds, bank_Metro, bank_Monzo, bank_RBS,
                type_of_card_Visa
            };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
    
            var inputs = new List<NamedOnnxValue>
                { NamedOnnxValue.CreateFromTensor("float_input", inputTensor) };

            string fraudIndicator;
            using (var results = _session.Run(inputs))
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                    .ToArray();
                if (prediction != null && prediction.Length > 0)
                {
                    fraudIndicator = fraud_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                    transaction.Fraud = fraudIndicator == "1";
                    _repo.UpdateTransaction(transaction);
                }
                else
                {
                    ViewBag.Prediction = "Error: Unable to make a prediction";
                }
            }
    
            _logger.LogInformation("Prediction executed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during prediction: {ex.Message}");
            ViewBag.Prediction = "Error during prediction.";
        }
    
        if (transaction.Fraud == false)
        {
            return View("Confirmation");
        }
        else
        {
            return View("PurchaseReview");
        }
    }

}