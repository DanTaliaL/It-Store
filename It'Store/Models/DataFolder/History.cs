using System;
using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }

        public List<Order> Orders { get; set; }
        public List<Request> Requests { get; set; }
    }
}
