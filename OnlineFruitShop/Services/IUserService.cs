using BusinessObject;
using System.Collections.Generic;

namespace Services
{
    public interface IUserService
    {
        User Login(string email, string password);
        void Register(User user);
        void UpdateUser(User user);
        User GetUserById(int id);
        List<User> GetAllUsers();
        void DeactivateUser(int id);
        bool IsEmailExist(string email);

        void UpdatePassword(int userId, string newPassword);
        User? GetUserByEmail(string email);
    }
}
