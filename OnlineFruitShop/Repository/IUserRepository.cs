using BusinessObject;
using System.Collections.Generic;

namespace Repository
{
    public interface IUserRepository
    {
        User Login(string email, string password);
        User GetUserById(int userId);
        bool IsEmailExist(string email);
        void RegisterUser(User user);
        void UpdateUser(User user);
        List<User> GetAllUsers();
        void DeactivateUser(int userId);
        void UpdatePassword(int userId, string newPassword);
        User? GetUserByEmail(string email);
    }
}
