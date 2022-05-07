namespace ItStore.Models.DataFolder
{
    public class ProductQuantity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int WareHouseId { get; set; }
        public WareHouse WareHouse { get; set; }
    }
}
