using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;
using Intex2024.Models;

namespace Intex2024.Components;

public class ProductFilterViewComponent : ViewComponent
{
    private IStoreRepository _repo;
    public ProductFilterViewComponent(IStoreRepository temp)
    {
        _repo = temp;
    }
    
    public IViewComponentResult Invoke()
    {
        var productTypes = _repo.Products
            .Select(x => x.Category1)
            .Distinct()
            .OrderBy(x => x);
        
        return View(productTypes);
    }
}