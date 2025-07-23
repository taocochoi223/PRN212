using BusinessObject;
using DataAccessLayer;
using System.Collections.Generic;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetAll() => CategoryDAO.GetAllCategories();

        public Category? GetById(int id) => CategoryDAO.GetCategoryById(id);

        public void Add(Category category) => CategoryDAO.AddCategory(category);

        public void Update(Category category) => CategoryDAO.UpdateCategory(category);

        public void Delete(int id) => CategoryDAO.DeleteCategory(id);
    }
}
