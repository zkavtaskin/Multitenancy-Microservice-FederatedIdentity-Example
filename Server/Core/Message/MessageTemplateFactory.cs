using Server.Core.Xml;
using System.Resources;

namespace Server.Core.Message.Email
{
    public class MessageTemplateFactory<TMessageTemplate>  
        where TMessageTemplate : MessageTemplate, new()
    {
        public  TMessageTemplate Create(ResourceManager resourceMgr, string key)
        {
            string templateContent = resourceMgr.GetString(key);
            return Serialization.Deserialize<TMessageTemplate>(templateContent);
        }
    }
}
