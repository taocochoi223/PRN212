using BusinessObject;
using Repository;
using System.Collections.Generic;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository iCategoryRepository;

        public CategoryService()
        {
            iCategoryRepository = new CategoryRepository();
        }

        public List<Category> GetAllCategories() => iCategoryRepository.GetAll();

        public Category? GetCategoryById(int id) => iCategoryRepository.GetById(id);

        public void AddCategory(Category category) => iCategoryRepository.Add(category);

        public void UpdateCategory(Category category) => iCategoryRepository.Update(category);

        public void DeleteCategory(int id) => iCategoryRepository.Delete(id);
    }
}
