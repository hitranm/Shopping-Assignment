using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository

    {
        public IEnumerable<OrderDetail> GetOrderDetailByID(string OrderID)
        => OrderDetailDAO.Instance.GetOrderDetailByID(OrderID);

        public IEnumerable<OrderDetail> GetOrderDetailByProductID(int ProductID)
        => OrderDetailDAO.Instance.GetOrderDetailByProductID(ProductID);

        public IEnumerable<OrderDetail> GetOrderDetails()
        => OrderDetailDAO.Instance.GetOrderDetailList();

        public void InsertOrderDetail(OrderDetail OrderDetail)
        => OrderDetailDAO.Instance.AddNew(OrderDetail);
    }
}
