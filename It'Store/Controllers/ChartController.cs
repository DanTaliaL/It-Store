using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;

namespace ItStore.Controllers
{
    public class ChartController : Controller
    {
        private DataContext Data { get; set; }
        private ChartViewModel ChartView { get; set; }
        public ChartController(DataContext Data, ChartViewModel ChartView)
        {
            this.Data = Data;
            this.ChartView = ChartView;
        }
        public IActionResult Chart()
        {
            var Chart = new ChartViewModel();
            Chart.Products = Data.Products.Select(x => new Product
            {
                Name = x.Name,
                Price = x.Price

            });
            Chart.MostBigPrice = 1;
            foreach (var q in Data.Products.OrderBy(a => a.Price))
            {
                Chart.MostBigPrice = q.Price;
            }

            Chart.MostSmallPrice = 1;
            foreach (var q in Data.Products.OrderByDescending(q => q.Price))
            {
                Chart.MostSmallPrice = q.Price;
            }
            Chart.SmallPriceFlag = Chart.MostSmallPrice;

            Chart.ProductQuantity = Chart.MostBigPrice / Chart.MostSmallPrice;

            Chart.FirstPoint = Chart.MostBigPrice * 0.30;
            Chart.SecondPoint = Chart.MostBigPrice * 0.35;
            Chart.ThirdPoint = Chart.MostBigPrice * 0.65;
            Chart.FourthPoint = Chart.MostBigPrice * 0.70;

            for (int i = 0; i < Chart.ProductQuantity; i++)
            {
                Chart.QuantityPoint.Add(Convert.ToString(Chart.MostSmallPrice) + ",");
                Chart.TopPrice.Add(Convert.ToString(1) + ",");
                Chart.MidlePrice.Add(Convert.ToString(0.75) + ","); //CultureInfo.InvariantCulture трансфер символов

                if (Chart.MostSmallPrice < Chart.FourthPoint + 1)
                {
                    Chart.First.Add($"{(Chart.MostSmallPrice - Chart.SmallPriceFlag) / (Chart.FourthPoint - Chart.SmallPriceFlag)}" + ",");
                }
                else
                {
                    Chart.First.Add(1 + ",");
                }

                if (Chart.MostSmallPrice > Chart.FirstPoint + 1)
                {
                    Chart.Second.Add((Chart.FirstPoint - Chart.MostSmallPrice) / (Chart.MostBigPrice - Chart.FirstPoint) + 1 + ",");
                }
                else
                {
                    Chart.Second.Add(1 + ",");
                }

                if (Chart.MostSmallPrice < Chart.SecondPoint)
                {
                    Chart.Third.Add((Chart.MostSmallPrice - Chart.SmallPriceFlag) / (Chart.SecondPoint - Chart.SmallPriceFlag) + ",");
                }
                else if (Chart.MostSmallPrice > Chart.ThirdPoint + 1)
                {
                    Chart.Third.Add((Chart.ThirdPoint - Chart.MostSmallPrice) / (Chart.MostBigPrice - Chart.ThirdPoint) + 1 + ",");
                }
                else
                {
                    Chart.Third.Add(1 + ",");
                }
                Chart.MostSmallPrice += Chart.SmallPriceFlag;
            }
            return View(Chart);
        }
    }
}
