using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class SupplierDAO
    {
        private static SupplierDAO instance = null;
        private static readonly object instanceLock = new object();
        private SupplierDAO() { }
        public static SupplierDAO Instance 
        { 
            get 
            { 
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SupplierDAO();
                    }
                }
                return instance; 
            } 
        }
        public IEnumerable<Supplier> GetSuppliers()
        {
            var supplier = new List<Supplier>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                supplier = context.Suppliers.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return supplier;
        }
        public Supplier GetSupplierById(int Id)
        {
            Supplier supplier = null;
            try
            {
                using var context = new NorthwindCopyDBContext();
                supplier = context.Suppliers.SingleOrDefault(s => s.SupplierId.Equals(Id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return supplier;
        }
    }
}
