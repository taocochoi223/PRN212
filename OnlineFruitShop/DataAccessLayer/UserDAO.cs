using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAO
    {
        public static User GetUserByLogin(string email, string password)
        {
            using var db = new OnlineFruitShopContext();
            return db.Users.FirstOrDefault(
                u => u.Email.ToLower().Equals(email.ToLower()) && 
                u.PasswordHash.Equals(password) && 
                u.IsActive == true);
        }


        public static User GetUserById(int userId)
        {
            using var db = new OnlineFruitShopContext();
            return db.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public static bool IsEmailExist(string email)
        {
            using var db = new OnlineFruitShopContext();
            return db.Users.Any(u => u.Email.ToLower().Equals(email.ToLower()));
        }

        public static void RegisterUser(User user)
        {
            using var db = new OnlineFruitShopContext();
            db.Users.Add(user);
            db.SaveChanges();
        }

        public static void UpdateUser(User user)
        {
            using var db = new OnlineFruitShopContext();
            db.Users.Update(user);
            db.SaveChanges();
        }
        public static List<User> GetAllUsers()
        {
            using var db = new OnlineFruitShopContext();
            return db.Users.ToList();
        }

        public static void DeactivateUser(int userId)
        {
            using var db = new OnlineFruitShopContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.IsActive = false;
                db.SaveChanges();
            }
        }
        public static void UpdatePassword(int userId, string newPassword)
        {
            using var db = new OnlineFruitShopContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.PasswordHash = newPassword;
                db.SaveChanges();
            }
        }

        public static User? GetUserByEmail(string email)
        {
            using var db = new OnlineFruitShopContext();
            return db.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

    }
}
