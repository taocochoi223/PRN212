using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class PasswordResetRequest
{
    public int ResetId { get; set; }

    public int UserId { get; set; }

    public DateTime? RequestedAt { get; set; }

    public bool? IsUsed { get; set; }

    public virtual User User { get; set; } = null!;
}
