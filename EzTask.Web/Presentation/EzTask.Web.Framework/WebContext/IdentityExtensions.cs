using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

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
