namespace ItStore.Models.DataFolder
{
    public class ChartViewModel
    {
        public double MostBigPrice { get; set; }
        public double MostSmallPrice { get; set; }
        public double ProductQuantity { get; set; }
        public double SmallPriceFlag { get; set; }
        public double FirstPoint { get; set; }
        public double SecondPoint { get; set; }
        public double ThirdPoint { get; set; }
        public double FourthPoint { get; set; }

        public List<string> First = new List<string>();
        public List<string> Second = new List<string>();
        public List<string> Third = new List<string>();
        public List<string> QuantityPoint = new List<string>();
        public List<string> MidlePrice = new List<string>();
        public List<string> TopPrice = new List<string>();
    
        public IQueryable<Product> Products { get; set; }
    }
}

