using System.Security.Principal;

namespace EzTask.Web.Framework.WebContext
{
    public static class IdentityExtensions
    {
        public static int AccountId(this IIdentity identity)
        {
            return Context.CurrentAccount.AccountId;
        }
    }
}
