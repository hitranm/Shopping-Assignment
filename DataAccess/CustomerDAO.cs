using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class CustomerDAO
    {
        private static CustomerDAO instance = null;
        private static readonly object instanceLock = new object(); 
        private CustomerDAO() { }
        public static CustomerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers;
            try
            {
                using var stock = new NorthwindCopyDBContext();
                customers = stock.Customers.OrderBy(c => c.CustomerId).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customers;
        }
        public Customer GetCustomerByID(string Id)
        {
            Customer customer;
            try
            {
                customer = GetCustomers().SingleOrDefault(c => c.CustomerId.Trim() == Id.Trim());

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }
        public Customer GetCustomerByEmail(string email)
        {
            Customer customer;
            try
            {
                customer = GetCustomers().SingleOrDefault(c => c.Email.Equals(email));

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }
        public Customer GetCustomerByPhone(string phone)
        {
            Customer customer;
            try
            {
                customer = GetCustomers().SingleOrDefault(c => c.Phone.Equals(phone));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }
        public void AddCustomer(Customer customer)
        {
            try
            {
                Customer cust = GetCustomerByID(customer.CustomerId);
                if (cust == null)
                {
                    using var stock = new NorthwindCopyDBContext();
                    stock.Customers.Add(customer);
                    stock.SaveChanges();
                }
                else
                {
                    throw new Exception("This customer already exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                Customer cust = GetCustomerByID(customer.CustomerId);
                if (cust != null)
                {
                    using var stock = new NorthwindCopyDBContext();
                    stock.Customers.Update(customer);
                    stock.SaveChanges();
                }
                else
                {
                    throw new Exception("This customer does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void RemoveCustomer(string Id)
        {
            try
            {
                Customer cust = GetCustomerByID(Id);
                if (cust != null)
                {
                    using var stock = new NorthwindCopyDBContext();
                    stock.Customers.Remove(cust);
                    stock.SaveChanges();
                }
                else
                {
                    throw new Exception("This customer does not existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
