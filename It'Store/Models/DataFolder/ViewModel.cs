namespace ItStore.Models.DataFolder
{
    public class ProductCartViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Picture> Pictures { get; set; }
        public IEnumerable<Options> Options { get; set; }
        public IEnumerable<Commentaries> Commentaries { get; set; }
        public int ProductID { get; set; }
        public string Comment { get; set; }
        public bool Grade { get; set; }
        public string UserName { get; set; }
    }

    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginInfo PaginInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string ProductModel { get; set; }
        public string Searchstring { get; set; }

    }

    public class HistoryViewModel
    {
        public IEnumerable<CartLine> CartLine { get; set; }
        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<Promotion> Promotion { get; set; }

    }

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



    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public IEnumerable<Promotion> Promotion { get; set; }
        public IEnumerable<ProductQuantity> ProductQuantity { get; set; }
        public string ReturnUrl { get; set; }
    }
}
