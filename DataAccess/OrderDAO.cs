using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                orders = context.Orders.Include(o => o.Customer).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public IEnumerable<Order> GetOrdersOfCustomer(string custId)
        {
            var orders = new List<Order>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                orders = context.Orders.Include(o => o.Customer).Where(o => o.CustomerId == custId).ToList();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public Order GetOrderById(string orderId)
        {
            Order order = null;
            try
            {
                using var context = new NorthwindCopyDBContext();
                order = context.Orders.Include(o => o.Customer).SingleOrDefault(o => o.OrderId == orderId);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;

        }
        public void AddOrder(Order order)
        {
            try {
                Order o = GetOrderById(order.OrderId);
                if (o == null)
                {
                    using var context = new NorthwindCopyDBContext();
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The order already existed");
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateOrder(Order order)
        {
            try
            {
                Order o = GetOrderById(order.OrderId);
                if (o != null)
                {
                    using var context = new NorthwindCopyDBContext();
                    context.Orders.Update(order);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order does not exist.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void Remove(string orderId)
        {
            try
            {
                Order o = GetOrderById(orderId);
                if (o != null)
                {
                    using var context = new NorthwindCopyDBContext();
                    context.Orders.Remove(o);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Order does not exist.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
