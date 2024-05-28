using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("expense-tracker-db");
            _users = database.GetCollection<User>("users");
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);
            user.Password = hashedPassword;

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public bool CheckPassword(User user, string password)
        {
            if (user == null)
            {
                return false;
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<bool> CheckUserExists(string email)
        {
            var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
            return user != null;
        }

        // Additional methods for user management...
    }
}
