using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public List<Request> Requests { get; set; }
    }
}
