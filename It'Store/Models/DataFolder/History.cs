using System;
using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class History
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductQuantity { get; set; }
        public DateTime DateTime { get; set; }
        public string Buyer { get; set; }
        public string TotalPrice { get; set; }
        public string? NamePromotion { get; set; }
        public string? PromotionDescription { get; set; }
        public string? PercentAge { get; set; }

    }
}
