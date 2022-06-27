using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Category> GetCategoryList()
        {
            var category = new List<Category>();
            try
            {
                using var context = new NorthwindCopyDBContext();
                category = context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }
        public Category GetCategoryByID(int categoryID)
        {
            Category cate = null;
            try
            {
                using var context = new NorthwindCopyDBContext();
                cate = context.Categories.SingleOrDefault(c => c.CategoryId == categoryID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return cate;
        }

    }
}
