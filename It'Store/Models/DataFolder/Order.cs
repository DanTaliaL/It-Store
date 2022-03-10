using ItStore.Models.DataFolder;
using System;
using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public DateTime TimeOrders { get; set; }
        public int ProductPrice { get; set; }
        public int OrderPrice { get; set; }
        public string DeliveryMethod { get; set; }
        public string PurshaceStatus { get; set; }
        public string Status { get; set; }
       
        public List<Product> Products { get; set; }
        public List<Request> Requests { get; set; }
        public List<Promotion> Promotion { get; set; }
        public List<History> History { get; set; }

    }
}
