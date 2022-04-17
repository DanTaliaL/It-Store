namespace ItStore.Models.DataFolder
{
    public class ProductCartViewModel
    {
        public IEnumerable<Product> Products { get; set;}
        public IEnumerable<Picture> Pictures { get; set;}
        public IEnumerable<Options> Options { get; set;}
        public IEnumerable<Commentaries> Commentaries { get; set;}
        public int ProductID { get; set;}
        public string Comment { get; set;}
        public string UserName { get; set;}
    }
}
