using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoViVoServer
{
    public class Utils
    {
        public static long currentTimeInMillis() {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
