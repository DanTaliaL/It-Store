using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }

        public bool PublicStatus { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ClosedDate { get; set; }

        public string Description { get; set; }
        public string PromotionCode { get; set; }

        public List<Order> Orders { get; set; }       
    }
}
