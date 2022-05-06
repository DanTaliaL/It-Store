namespace ItStore.Models.DataFolder
{
    public class HistoryViewModel
    {
        public IEnumerable<CartLine> CartLine { get; set; }
        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<Promotion> Promotion { get; set; }

    }
}
