using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Core.Time
{
    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        public override DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
