using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intex2024.Components;

public class ColorFilterViewComponent : ViewComponent
{
    private IStoreRepository _repo;
    public ColorFilterViewComponent(IStoreRepository temp)
    {
        _repo = temp;
    }
    
    public IViewComponentResult Invoke()
    {
        // ViewBag.SelectedColors = RouteData?.Values["color"];

        var colors = _repo.Products
            .ToList()
            .SelectMany(x => new[] { x.PrimaryColor, x.SecondaryColor })
            .Distinct()
            .OrderBy(x => x)
            .AsQueryable(); // Convert the List<string?> to IQueryable<string?>

        return View(colors);
    }



}