using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetProductList()
        {
            var products = new List<Product>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                products = context.Products.Include(p => p.Category).Include(p => p.Supplier).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return products;

        }

        public Product GetProductByID(int ProductID)
        {
            Product product = null;
            try
            {
                using var context = new NorthwindCopyDBContext();
                product = context.Products.Include(p=>p.Category)
                    .Include(p=>p.Supplier)
                    .SingleOrDefault(c => c.ProductId == ProductID);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return product;
        }

        public void AddNew(Product Product)
        {
            try
            {
                Product product = GetProductByID(Product.ProductId);
                if (product == null)
                {
                    using var context = new NorthwindCopyDBContext();
                    context.Products.Add(Product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product is already exist.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Update(Product Product)
        {
            try
            {
                Product product = GetProductByID(Product.ProductId);
                if (product != null)
                {
                    using var context = new NorthwindCopyDBContext();
                    context.Products.Update(Product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product does not already exist.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(int ProductId)
        {
            try
            {
                Product product = GetProductByID(ProductId);
                if (product != null)
                {
                    using var context = new NorthwindCopyDBContext();
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product does not already exist.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
