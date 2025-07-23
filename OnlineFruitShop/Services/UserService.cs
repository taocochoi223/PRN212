using BusinessObject;
using Repository;
using System;
using System.Collections.Generic;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository iUserRepository;

        public UserService()
        {
            iUserRepository = new UserRepository();
        }

        public User Login(string email, string password)
        {
            return iUserRepository.Login(email, password);
        }

        public void Register(User user)
        {
            if (iUserRepository.IsEmailExist(user.Email))
            {
                throw new Exception("Email already exists.");
            }
            iUserRepository.RegisterUser(user);
        }

        public void UpdateUser(User user)
        {
            iUserRepository.UpdateUser(user);
        }
        public User GetUserById(int id)
        {
            return iUserRepository.GetUserById(id);
        }

        public List<User> GetAllUsers()
        {
            return iUserRepository.GetAllUsers();
        }

        public void DeactivateUser(int id)
        {
            iUserRepository.DeactivateUser(id);
        }

        public bool IsEmailExist(string email)
        {
            return iUserRepository.IsEmailExist(email);
        }

        // ✅ THÊM 2 HÀM DƯỚI ĐÂY
        public void UpdatePassword(int userId, string newPassword)
        {
            iUserRepository.UpdatePassword(userId, newPassword);
        }

        public User? GetUserByEmail(string email)
        {
            return iUserRepository.GetUserByEmail(email);
        }
    }
}
