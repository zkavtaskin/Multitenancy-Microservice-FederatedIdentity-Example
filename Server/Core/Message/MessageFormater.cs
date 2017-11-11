using System.Collections.Generic;

namespace Server.Core.Message
{
    public abstract class MessageFormater<TMessageTemplate, TMessage>
        where TMessageTemplate : MessageTemplate, new()
        where TMessage : Message, new()
    {
        public abstract TMessage Format(TMessageTemplate template, List<KeyValuePair<string, string>> tokens);
    }
}
