using BusinessObject;

namespace Services
{
    public interface IPasswordResetService
    {
        void RequestReset(int userId);
        PasswordResetRequest? GetActiveRequest(int userId);
        void CompleteReset(int resetId);
    }
}
