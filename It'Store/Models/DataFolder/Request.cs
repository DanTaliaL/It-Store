using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductPrice { get; set; }
        public string PurshaceMethod { get; set; }
        public string Quantity { get; set; }

        public List<Supplier> Suppliers { get; set; }
        public List<Order> Orders { get; set; }
    }
}