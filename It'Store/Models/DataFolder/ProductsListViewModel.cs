namespace ItStore.Models.DataFolder
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginInfo PaginInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string ProductModel { get; set; }
        public string Searchstring { get; set; }

    }
}
