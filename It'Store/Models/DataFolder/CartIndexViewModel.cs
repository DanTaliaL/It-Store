namespace ItStore.Models.DataFolder
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public IEnumerable<Promotion> Promotion { get; set; }
        public string ReturnUrl { get; set; }
    }
}
