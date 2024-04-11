using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex2024.Models;
using Intex2024.Models.ViewModels;

using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Elfie.Serialization;
using System.Drawing.Printing;
using System.Drawing;
using Intex2024.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intex2024.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IStoreRepository _repo;
    private readonly InferenceSession _session;
    private readonly IntexStoreContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbcontext;


    public HomeController(IntexStoreContext context, 
        ILogger<HomeController> logger, 
        IStoreRepository temp, 
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext)
    {
        _context = context;
        _logger = logger;
        _repo = temp;
        _userManager = userManager;
        _dbcontext = applicationDbContext;
        try
        {
            _session = new InferenceSession("C: /Users/eliasbaker/Source/Repos/Intex2024/Intex2024/fraudModel.onnx");
            _logger.LogInformation("ONNX model loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading the ONNX model: {ex.Message}");
        }
    }


    public IActionResult Index(string? cat, string? color, int pageSize = 5, int pageNum = 1)
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
            products = products
        };

        return View(viewModel);
    }



    public ActionResult ProductDetails(int id)
    {
        // Query for the product and each related item
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
            RelatedProduct3 = relatedProduct3
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
        if (User.Identity.IsAuthenticated)
        {
            var currentuser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userrole = _dbcontext.ApplicationUserRoles.Include(c=>c.User).Include(c=>c.Role).Where(c=>c.UserId == currentuser.Id).FirstOrDefault();  
            var user = userrole.User;  
            var role = userrole.Role;
            cust.UserId = currentuser.Id;
        }
        
        if (ModelState.IsValid)
        {
            _repo.AddCustomer(cust);
            return RedirectToAction("Index");
        }
        else
        {
            return View("UserInfo", cust);
        }
    }


    // public IActionResult Predict(int customer_ID, int amount, string day_of_week, string entry_mode,
    //     string type_of_transaction, string country_of_transaction, string shipping_address, string bank,
    //     string type_of_card)
    // {
    //     int day_of_week_Mon = 0;
    //     int day_of_week_Sat = 0;
    //     int day_of_week_Sun = 0;
    //     int day_of_week_Thu = 0;
    //     int day_of_week_Tue = 0;
    //     int day_of_week_Wed = 0;
    //     int entry_mode_PIN = 0;
    //     int entry_mode_Tap = 0;
    //     int type_of_transaction_Online = 0;
    //     int type_of_transaction_POS = 0;
    //     int country_of_transaction_India = 0;
    //     int country_of_transaction_Russia = 0;
    //     int country_of_transaction_USA = 0;
    //     int country_of_transaction_United_Kingdom = 0;
    //     int shipping_address_India = 0;
    //     int shipping_address_Russia = 0;
    //     int shipping_address_USA = 0;
    //     int shipping_address_United_Kingdom = 0;
    //     int bank_HSBC = 0;
    //     int bank_Halifax = 0;
    //     int bank_Lloyds = 0;
    //     int bank_Metro = 0;
    //     int bank_Monzo = 0;
    //     int bank_RBS = 0;
    //     int type_of_card_Visa = 0;
    //
    //     if (day_of_week == "Mon")
    //     {
    //         day_of_week_Mon = 1;
    //     }
    //     else if (day_of_week == "Tue")
    //     {
    //         day_of_week_Tue = 1;
    //     }
    //     else if (day_of_week == "Wed")
    //     {
    //         day_of_week_Wed = 1;
    //     }
    //     else if (day_of_week == "Thu")
    //     {
    //         day_of_week_Thu = 1;
    //     }
    //     else if (day_of_week == "Sat")
    //     {
    //         day_of_week_Sat = 1;
    //     }
    //     else if (day_of_week == "Sun")
    //     {
    //         day_of_week_Sun = 1;
    //     }
    //
    //     if (entry_mode == "PIN")
    //     {
    //         entry_mode_PIN = 1;
    //     }
    //     else if (entry_mode == "Tap")
    //     {
    //         entry_mode_Tap = 1;
    //     }
    //
    //     if (type_of_transaction == "Online")
    //     {
    //         type_of_transaction_Online = 1;
    //     }
    //     else if (type_of_transaction == "POS")
    //     {
    //         type_of_transaction_POS = 1;
    //     }
    //
    //     if (country_of_transaction == "India")
    //     {
    //         country_of_transaction_India = 1;
    //     }
    //     else if (country_of_transaction == "Russia")
    //     {
    //         country_of_transaction_Russia = 1;
    //     }
    //     else if (country_of_transaction == "USA")
    //     {
    //         country_of_transaction_USA = 1;
    //     }
    //     else if (country_of_transaction == "United Kingdom")
    //     {
    //         country_of_transaction_United_Kingdom = 1;
    //     }
    //
    //     if (shipping_address == "India")
    //     {
    //         shipping_address_India = 1;
    //     }
    //     else if (shipping_address == "Russia")
    //     {
    //         shipping_address_Russia = 1;
    //     }
    //     else if (shipping_address == "USA")
    //     {
    //         shipping_address_USA = 1;
    //     }
    //     else if (shipping_address == "United Kingdom")
    //     {
    //         shipping_address_United_Kingdom = 1;
    //     }
    //
    //     if (bank == "HSBC")
    //     {
    //         bank_HSBC = 1;
    //     }
    //     else if (bank == "Halifax")
    //     {
    //         bank_Halifax = 1;
    //     }
    //     else if (bank == "Lloyds")
    //     {
    //         bank_Lloyds = 1;
    //     }
    //     else if (bank == "Metro")
    //     {
    //         bank_Metro = 1;
    //     }
    //     else if (bank == "Monzo")
    //     {
    //         bank_Monzo = 1;
    //     }
    //     else if (bank == "RBS")
    //     {
    //         bank_RBS = 1;
    //     }
    //
    //     if (type_of_card == "Visa")
    //     {
    //         type_of_card_Visa = 1;
    //     }
    //
    //     var fraud_dict = new Dictionary<int, string>()
    //     {
    //         { 0, "No" },
    //         { 1, "Yes" }
    //     };
    //
    //     try
    //     {
    //         var input = new List<float>
    //         {
    //             customer_ID, amount, day_of_week_Mon, day_of_week_Sat,
    //             day_of_week_Sun, day_of_week_Thu, day_of_week_Tue,
    //             day_of_week_Wed, entry_mode_PIN, entry_mode_Tap,
    //             type_of_transaction_Online, type_of_transaction_POS,
    //             country_of_transaction_India, country_of_transaction_Russia,
    //             country_of_transaction_USA, country_of_transaction_United_Kingdom,
    //             shipping_address_India, shipping_address_Russia,
    //             shipping_address_USA, shipping_address_United_Kingdom, bank_HSBC,
    //             bank_Halifax, bank_Lloyds, bank_Metro, bank_Monzo, bank_RBS,
    //             type_of_card_Visa
    //         };
    //         var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
    //
    //         var inputs = new List<NamedOnnxValue>
    //             { NamedOnnxValue.CreateFromTensor("float_input", inputTensor) };
    //
    //         using (var results = _session.Run(inputs))
    //         {
    //             var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
    //                 .ToArray();
    //             if (prediction != null && prediction.Length > 0)
    //             {
    //                 var fraudIndicator = fraud_dict.GetValueOrDefault((int)prediction[0], "Unknown");
    //                 ViewBag.Prediction = fraudIndicator;
    //             }
    //             else
    //             {
    //                 ViewBag.Prediction = "Error: Unable to make a prediction";
    //             }
    //         }
    //
    //         _logger.LogInformation("Prediction executed successfully.");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError($"Error during prediction: {ex.Message}");
    //         ViewBag.Prediction = "Error during prediction.";
    //     }
    //
    //     if (ViewBag.Prediction == 0)
    //     {
    //         // return View("PurchaseConfirmation");
    //     }
    //     else
    //     {
    //         // return View("Purchase Review");
    //     }
    // }

}