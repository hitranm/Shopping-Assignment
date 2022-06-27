using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetOrdersOfMember(string custId);
        Order GetOrderByID(string OrderID);
        void InsertOrder(Order order);
        void DeleteOrder(string OrderID);
        void UpdateOrder(Order order);
    }
}
