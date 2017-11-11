using System;
using System.Collections.Generic;
using System.Reflection;

namespace Server.Core.Message
{
    public class TokenGenerator
    {
        public List<KeyValuePair<string, string>> Generate(object anonymous)
        {
            List<KeyValuePair<string, string>> tokensWithValues = new List<KeyValuePair<string, string>>();

            if (anonymous != null)
            {
                Type type = anonymous.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                object value;

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    value = propertyInfo.GetValue(anonymous, null);

                    if (value != null)
                    {
                        tokensWithValues.Add(new KeyValuePair<string, string>(String.Format("@{0}@", propertyInfo.Name), value.ToString()));
                    }
                }
            }

            return tokensWithValues;
        }
    }
}
