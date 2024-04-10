using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intex2024.Components
{
    public class FraudFilterViewComponent : ViewComponent
    {
        private IStoreRepository _repo;
        public FraudFilterViewComponent(IStoreRepository temp)
        {
            _repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            var orders = _repo.Transactions
                .ToList()
                .Select(x => x.Fraud)
                .Distinct()
                .OrderBy(x => x)
                .AsQueryable();

            return View(orders);
        }
    }
}
