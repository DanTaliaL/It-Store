namespace ItStore.Models.DataFolder
{
    public class Commentaries
    {
        public int Id { get; set; }
        public string Comment   { get; set; }
        public bool Grade { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }


        public int ProductID { get; set; }
        public Product Product { get; set; }


    }
}
