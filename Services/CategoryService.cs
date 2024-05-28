using MongoDB.Driver;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("expense-tracker-db");
            _categories = database.GetCollection<Category>("Categories");
        }

        public async Task<Category> AddCategoryAsync(string name, string userId) {
            var category = new Category
            {
                Name = name,
                UserId = userId
            };

            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
        {
            var categories = await _categories.Find(category => category.UserId == userId).ToListAsync();
            return categories;
        }

        public async Task DeleteCategoryAsync(string categoryId)
        {
            var deleteResult = await _categories.DeleteOneAsync(categoryId);

            if (!deleteResult.IsAcknowledged || deleteResult.DeletedCount == 0)
            {
                throw new InvalidOperationException("Error deleting category or category not found");
            }
        }
    }
}
