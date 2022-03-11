using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.EntityFrameworkCore;

namespace It_Store.Models.DataFolder
{
         public interface IOrderRepository
        {
            IQueryable<Order> Orders { get; }
            void SaveOrder(Order order);
        }
    
 
    public class EFOrderRepository : IOrderRepository
    {
        private DataContext  Data { get; set; }
        public EFOrderRepository(DataContext DC)=>Data = DC;

        public IQueryable<Order> Orders => Data.Orders
            .Include(q => q.Lines)
            .ThenInclude(q => q.Product);

        

        public void SaveOrder(Order order)
        {
            Data.AttachRange(order.Lines.Select(q=>q.Product));
            if (order.Id==0)
            {
                Data.Orders.Add(order);
            }
            Data.SaveChanges();
        }
    }
}
