using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        public void CreateResetRequest(PasswordResetRequest request)
            => PasswordResetRequestDAO.CreateResetRequest(request);

        public PasswordResetRequest? GetActiveRequestByUserId(int userId)
            => PasswordResetRequestDAO.GetActiveRequestByUserId(userId);

        public void MarkAsUsed(int resetId)
            => PasswordResetRequestDAO.MarkAsUsed(resetId);
    }
}
