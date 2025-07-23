using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataAccessLayer
{
    public class PasswordResetRequestDAO
    {
        public static void CreateResetRequest(PasswordResetRequest request)
        {
            using var context = new OnlineFruitShopContext();
            context.PasswordResetRequests.Add(request);
            context.SaveChanges();
        }

        public static PasswordResetRequest? GetActiveRequestByUserId(int userId)
        {
            using var context = new OnlineFruitShopContext();
            return context.PasswordResetRequests
                          .Where(r => r.UserId == userId && r.IsUsed == false)
                          .OrderByDescending(r => r.RequestedAt)
                          .FirstOrDefault();
        }

        public static void MarkAsUsed(int resetId)
        {
            using var context = new OnlineFruitShopContext();
            var request = context.PasswordResetRequests.FirstOrDefault(r => r.ResetId == resetId);
            if (request != null)
            {
                request.IsUsed = true;
                context.SaveChanges();
            }
        }
    }
}
