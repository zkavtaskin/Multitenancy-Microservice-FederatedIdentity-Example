using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Core.Time
{
    public class TestTimeProvider : TimeProvider
    {
        private DateTime utc;
        private DateTime now;

        public override DateTime UtcNow
        {
            get
            {
                return this.utc;
            }
        }

        public override DateTime Now
        {
            get
            {
                return this.now;
            }
        }

        public TestTimeProvider(DateTime utc, DateTime now)
        {
            this.utc = utc;
            this.now = now;
        }

        public TestTimeProvider(DateTime utcAndNow)
        {
            this.utc = utcAndNow;
            this.now = utcAndNow;
        }
    }
}
