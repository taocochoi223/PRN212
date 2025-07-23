using BusinessObject;
using Repository;
using System;

namespace Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordResetRepository _repo;

        public PasswordResetService()
        {
            _repo = new PasswordResetRepository();
        }

        public void RequestReset(int userId)
        {
            var request = new PasswordResetRequest
            {
                UserId = userId,
                RequestedAt = DateTime.Now,
                IsUsed = false
            };

            _repo.CreateResetRequest(request);

            // TODO: Gửi OTP qua email ở đây (phần gửi sẽ xử lý ở Presentation)
        }

        public PasswordResetRequest? GetActiveRequest(int userId)
            => _repo.GetActiveRequestByUserId(userId);

        public void CompleteReset(int resetId)
            => _repo.MarkAsUsed(resetId);
    }
}
