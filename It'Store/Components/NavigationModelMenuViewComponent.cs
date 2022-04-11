using ItStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItStore.Components
{
    public class NavigationModelMenuViewComponent : ViewComponent
    {
        private DataContext Data { get; set; }
        public NavigationModelMenuViewComponent(DataContext Data)
        {
            this.Data = Data;
        }
        public IViewComponentResult Invoke()
        {
            return View(Data.Products
                .Select(q => q.Model)
                .Distinct()
                .OrderBy(q => q));
        }
    }
}