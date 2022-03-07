using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Number { get; set; }
        public string Adress { get; set; }
        public string Type { get; set; }
        
        public List<Request> Requests { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
       
    }
}
