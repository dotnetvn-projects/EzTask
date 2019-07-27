using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Common
{
    public static class StringUtilities
    {
        public static string CreateFakeEmail()
        {
            return Guid.NewGuid().ToString() + "@fakemail.com";
        }
    }
}
