using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(int ProductID)
        => ProductDAO.Instance.Remove(ProductID);

        public Product GetProductByID(int ProductID)
        => ProductDAO.Instance.GetProductByID(ProductID);

        public IEnumerable<Product> GetProducts()
        => ProductDAO.Instance.GetProductList();

        public void InsertProduct(Product Product)
        => ProductDAO.Instance.AddNew(Product);

        public void UpdateProduct(Product Product)
        => ProductDAO.Instance.Update(Product);
    }
}
