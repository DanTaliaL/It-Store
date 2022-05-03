namespace ItStore.Models.DataFolder
{
    public class HistoryViewModel
    {
        public IEnumerable<CartLine> CartLine { get; set; }
        public IEnumerable<Order> Order { get; set; }

    }
}
