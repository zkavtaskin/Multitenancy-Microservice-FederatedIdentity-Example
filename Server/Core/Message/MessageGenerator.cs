using System.Collections.Generic;
using System.Resources;

namespace Server.Core.Message.Email
{
    public class MessageGenerator<TMessageTemplate, TMessage> 
        where TMessageTemplate : MessageTemplate, new()
        where TMessage : Message, new()
    {
        MessageFormater<TMessageTemplate, TMessage> formater;
        MessageTemplateFactory<TMessageTemplate> templateFactory;
        TokenGenerator tokenGenerator;

        public MessageGenerator(MessageFormater<TMessageTemplate, TMessage> formater, 
            MessageTemplateFactory<TMessageTemplate> templateFactory, TokenGenerator tokenGenerator)
        {
            this.formater = formater;
            this.templateFactory = templateFactory;
            this.tokenGenerator = tokenGenerator;
        }

        public TMessage Generate(ResourceManager resource, string messageTemplateKey, object anonymous)
        {
            List<KeyValuePair<string, string>> tokensWithValues = this.tokenGenerator.Generate(anonymous);
            TMessageTemplate msgTemplate = this.templateFactory.Create(resource, messageTemplateKey);
            TMessage msg = this.formater.Format(msgTemplate, tokensWithValues);
            return msg;
        }
    }
}
