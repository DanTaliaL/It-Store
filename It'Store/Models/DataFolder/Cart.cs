using ItStore.Models.DataFolder;

namespace ItStore.Models.DataFolder
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
    public class Cart
    {
        private List<CartLine> LineCollection = new List<CartLine>();
        public virtual void AddItem (Product product, int quantity)
        {
            CartLine Line = LineCollection.Where(q => q.Product.Id == product.Id).FirstOrDefault();
            if (Line == null)
            { 
                LineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                Line.Quantity += quantity;
            } 
            
        }

        public virtual void RemoveLine(Product product)=>LineCollection.RemoveAll(q=>q.Product.Id==product.Id);
        public virtual decimal ComputeTotalValue() => LineCollection.Sum(q => q.Product.Price * q.Quantity);
        public virtual void Clear()=> LineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => LineCollection;
    }
}
