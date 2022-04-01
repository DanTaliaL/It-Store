using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ItStore.Models.DataFolder;
using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }      
        public bool Aviability { get; set; }
        public int Quantity { get; set; }
        public string Model { get; set; }
        public string Categories { get; set; }
        public string SEO { get; set; }
        public byte[] Image { get; set; }

        public List<Order> Orders { get; set; }
        
        public int WareHouseId { get; set; }
        public WareHouse WareHouse { get; set; }

        public List<Options> Options { get; set; } 

        public List<Supplier> Suppliers { get; set; }
        public List<Picture> Pictures { get; set; }
        
    }
}
