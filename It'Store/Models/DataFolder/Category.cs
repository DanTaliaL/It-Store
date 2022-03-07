
namespace ItStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
        public string SEO { get; set; }  
        
        public List<Product> Products { get; set; }
    }
}
