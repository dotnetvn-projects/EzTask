using System.Collections.Generic;

namespace EzTask.Framework.ModelValidatorAttributes
{
    internal class ValidatorUtils
    {
        public static bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
