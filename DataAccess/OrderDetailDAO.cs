using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetailList()
        {
            var members = new List<OrderDetail>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                members = context.OrderDetails.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return members;

        }

        public IEnumerable<OrderDetail> GetOrderDetailByID(string OrderID)
        {
            var orderDetails = new List<OrderDetail>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                orderDetails = context.OrderDetails.Where(o => o.OrderId == OrderID).Include(o => o.Product).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return orderDetails;
        }
        public IEnumerable<OrderDetail> GetOrderDetailByProductID(int ProductID)
        {
            var orderDetails = new List<OrderDetail>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                orderDetails = context.OrderDetails.Where(c => c.ProductId == ProductID).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return orderDetails;
        }
        public void AddNew(OrderDetail OrderDetail)
        {
            try
            {
                using var context = new NorthwindCopyDBContext();
                context.OrderDetails.Add(OrderDetail);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
