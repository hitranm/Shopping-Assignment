using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public void AddCustomer(Customer customer)
        => CustomerDAO.Instance.AddCustomer(customer);

        public void DeleteCustomer(String customerId)
        => CustomerDAO.Instance.RemoveCustomer(customerId);

        public Customer GetCustomerByEmail(string email)
        => CustomerDAO.Instance.GetCustomerByEmail(email);

        public Customer GetCustomerById(string id)
        => CustomerDAO.Instance.GetCustomerByID(id);

        public Customer GetCustomerByPhone(string phone)
        => CustomerDAO.Instance.GetCustomerByPhone(phone);

        public IEnumerable<Customer> GetCustomers()
        => CustomerDAO.Instance.GetCustomers();

        public void UpdateCustomer(Customer customer)
        => CustomerDAO.Instance.UpdateCustomer(customer);
    }
}
