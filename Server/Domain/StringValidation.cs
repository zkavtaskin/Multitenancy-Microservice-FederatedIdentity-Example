
namespace Server.Domain
{
    public class StringValidation
    {
        public static string Email = @"^([a-zA-Z0-9_\-\.'+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public static string TenantName = @"^[A-Za-z0-9 ]{1,30}$";
        public static string TenantNameFriendly = @"^[a-z0-9_]{1,30}$";
    }
}
