using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> GetCategories() => CategoryDAO.Instance.GetCategoryList();
     

        public Category GetCategoryByID(int cateId) => CategoryDAO.Instance.GetCategoryByID(cateId);
       
    }
}
