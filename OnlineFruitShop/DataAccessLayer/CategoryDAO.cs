using BusinessObject;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class CategoryDAO
    {
        public static List<Category> GetAllCategories()
        {
            using var db = new OnlineFruitShopContext();
            return db.Categories.ToList();
        }

        public static Category? GetCategoryById(int id)
        {
            using var db = new OnlineFruitShopContext();
            return db.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public static void AddCategory(Category category)
        {
            using var db = new OnlineFruitShopContext();
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public static void UpdateCategory(Category category)
        {
            using var db = new OnlineFruitShopContext();
            var existing = db.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (existing != null)
            {
                existing.CategoryName = category.CategoryName;
                existing.Description = category.Description;
                db.SaveChanges();
            }
        }

        public static void DeleteCategory(int id)
        {
            using var db = new OnlineFruitShopContext();
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
            }
        }
    }
}
