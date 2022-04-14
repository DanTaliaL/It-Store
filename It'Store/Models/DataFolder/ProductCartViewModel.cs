namespace ItStore.Models.DataFolder
{
    public class ProductCartViewModel
    {
        public IEnumerable<Product> Products { get; set;}
        public IEnumerable<Picture> Pictures { get; set;}
        public IEnumerable<Options> Options { get; set;}
        public int ProductId { get; set; }
    }
}
