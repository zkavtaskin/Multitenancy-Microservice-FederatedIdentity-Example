using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Core.Time
{
    public abstract class TimeProvider
    {
        private static TimeProvider current;

        static TimeProvider()
        {
            TimeProvider.current =
                new DefaultTimeProvider();
        }

        public static TimeProvider Current
        {
            get
            {
                return TimeProvider.current;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                TimeProvider.current = value;
            }
        }

        public abstract DateTime UtcNow { get; } 

        public abstract DateTime Now { get; }

        public static void ResetToDefault()
        {
            TimeProvider.current =
                new DefaultTimeProvider();
        }
    }
}
