namespace ItStore.Models.DataFolder
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
