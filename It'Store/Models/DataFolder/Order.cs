using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItStore.Models.DataFolder
{
    public class Order
    {
        [BindNever]
        public int Id { get; set; }
        [BindNever]
        public ICollection<CartLine>? Lines { get; set; }
        [Required(ErrorMessage = "Поле имя не может быть пустым")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Название улицы не может быть пустым")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Название города не может быть пустым")]
        public string City { get; set; }

        //old column
        public DateTime TimeOrders { get; set; }
        //public int ProductPrice { get; set; }
        //public int OrderPrice { get; set; }
        public string DeliveryMethod { get; set; }
        //public string PurshaceStatus { get; set; }
        //public string Status { get; set; }

        [BindNever]
        public List<Product>? Products { get; set; }
        [BindNever]
        public List<Request>? Requests { get; set; }
        [BindNever]
        public List<Promotion>? Promotion { get; set; }
        [BindNever]
        public List<History>? History { get; set; }

    }
}
