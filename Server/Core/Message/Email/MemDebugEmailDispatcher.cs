using System.Collections.Generic;
using log4net;

namespace Server.Core.Message.Email
{
    public class MemDebugEmailDispatcher : IEmailDispatcher
    {
        List<Email> messages = new List<Email>();
        ILog log;

        public MemDebugEmailDispatcher(ILog log)
        {
            this.log = log;
        }

        public void Dispatch(Email message)
        {
            this.log.Info(message.Message.BodyPlainText);
            this.messages.Add(message);
        }
    }
}
