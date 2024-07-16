using System.ComponentModel;

namespace RPSSL.Web.Domain.Enums;

public enum Role
{
    [Description("User")]
    User = 1,
    [Description("Admin")]
    Admin = 2,
    [Description("Agent")]
    Agent = 3,
    [Description("Owner")]
    Owner = 4
}
