using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            int MostBigPrice = 1;
            foreach (var q in Data.Products.OrderBy(a => a.Price))
            {
                MostBigPrice = q.Price;
            }

            int MostSmallPrice = 1;
            foreach (var q in Data.Products.OrderByDescending(q => q.Price))
            {
                MostSmallPrice = q.Price;
            }
            int SmallPriceFlag = MostSmallPrice;

            int ProductQuantity = MostBigPrice / MostSmallPrice;

            for (int i = 0; i <= ProductQuantity; i++)
            {
                ChartView.QuantityPoint.Add(Convert.ToString(MostSmallPrice) + ",");
                ChartView.MidlePrice.Add(Convert.ToString(1) + ",");
                ChartView.TopPrice.Add(Convert.ToString(0.75) + ",");
                if (MostSmallPrice < 8700)
                {
                    ChartView.First.Add($"{(MostSmallPrice - SmallPriceFlag) / (8600.0 - SmallPriceFlag)}" + ",");
                }
                else
                {
                    ChartView.First.Add(1 + ",");
                }
                if (MostSmallPrice > 3600)
                {
                    ChartView.Second.Add((3600.0 - MostSmallPrice) / (MostBigPrice - 3600.0) + 1 + ",");
                }
                else
                {
                    ChartView.Second.Add(1 + ",");
                }
                if (MostSmallPrice < 4300)
                {
                    ChartView.Third.Add((MostSmallPrice - SmallPriceFlag) / (4200.0 - SmallPriceFlag) + ",");
                }
                else if (MostSmallPrice > 7900)
                {
                    ChartView.Third.Add((7800.0 - MostSmallPrice) / (MostBigPrice - 7800.0) + 1 + ",");
                }
                else
                {
                    ChartView.Third.Add(1 + ",");
                }
                MostSmallPrice += SmallPriceFlag;                
            }
            return View(ChartView);
        }
    }
}
