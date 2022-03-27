using ItStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItStore.Components
{
    public class NavigationMenuViewComponent :ViewComponent
    {
        private DataContext Data { get; set; }
        public NavigationMenuViewComponent(DataContext Data)
        {
            this.Data = Data;
        }
        public IViewComponentResult Invoke()
        {
            return View(Data.Products
                .Select(q => q.Categories)
                .Distinct()
                .OrderBy(q => q));
        }
    }
}
