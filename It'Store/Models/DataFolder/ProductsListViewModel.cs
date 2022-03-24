namespace ItStore.Models.DataFolder
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set;}
        public PaginInfo PaginInfo { get; set;}
    }
}
