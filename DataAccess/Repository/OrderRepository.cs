using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void DeleteOrder(string OrderID)
        => OrderDAO.Instance.Remove(OrderID);

        public Order GetOrderByID(string OrderID)
        => OrderDAO.Instance.GetOrderById(OrderID);

        public IEnumerable<Order> GetOrders()
        => OrderDAO.Instance.GetOrders();

        public IEnumerable<Order> GetOrdersOfMember(string custId)
        => OrderDAO.Instance.GetOrdersOfCustomer(custId);

        public void InsertOrder(Order order)
        => OrderDAO.Instance.AddOrder(order);

        public void UpdateOrder(Order order)
        => OrderDAO.Instance.UpdateOrder(order);
    }
}
