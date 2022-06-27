using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        public Supplier GetSupplierById(int supplierId)
        => SupplierDAO.Instance.GetSupplierById(supplierId);

        public IEnumerable<Supplier> GetSuppliers()
        => SupplierDAO.Instance.GetSuppliers();
    }
}
