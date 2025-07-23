using BusinessObject;
using DataAccessLayer;
using System.Collections.Generic;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public void DeactivateUser(int userId) => UserDAO.DeactivateUser(userId);

        public List<User> GetAllUsers() => UserDAO.GetAllUsers();

        public User GetUserById(int userId) => UserDAO.GetUserById(userId);

        public bool IsEmailExist(string email) => UserDAO.IsEmailExist(email);

        public User Login(string email, string password) => UserDAO.GetUserByLogin(email, password);

        public void RegisterUser(User user) => UserDAO.RegisterUser(user);
        public void UpdateUser(User user) => UserDAO.UpdateUser(user);

        public void UpdatePassword(int userId, string newPassword) => UserDAO.UpdatePassword(userId, newPassword);

        public User? GetUserByEmail(string email) => UserDAO.GetUserByEmail(email);
    }
}
