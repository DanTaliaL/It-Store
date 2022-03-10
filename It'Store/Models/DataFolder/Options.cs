using System.Collections.Generic;

namespace ItStore.Models.DataFolder
{
    public class Options
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Volume { get; set; }
        public int ClockFrequency { get; set; }
        public int Size { get; set; }
        public string Model { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
