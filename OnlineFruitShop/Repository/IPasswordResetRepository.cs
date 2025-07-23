using BusinessObject;

namespace Repository
{
    public interface IPasswordResetRepository
    {
        void CreateResetRequest(PasswordResetRequest request);
        PasswordResetRequest? GetActiveRequestByUserId(int userId);
        void MarkAsUsed(int resetId);
    }
}
