using ItStore.Models.DataFolder;
using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Supplier> Suppliers { get; set; }
    }
}
