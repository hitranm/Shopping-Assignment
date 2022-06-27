using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderDetails();
        IEnumerable<OrderDetail> GetOrderDetailByID(string OrderID);
        IEnumerable<OrderDetail> GetOrderDetailByProductID(int ProductID);
        void InsertOrderDetail(OrderDetail OrderDetail);

    }
}
