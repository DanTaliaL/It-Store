using System.Collections.Generic;
namespace ItStore.Models.DataFolder
{
    public class WareHouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }

        public List<ProductQuantity> ProductQuantities { get; set; }
        public List<Product> Product { get; set; }
    }
}
