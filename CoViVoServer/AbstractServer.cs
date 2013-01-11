using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WrapperLib;

namespace CoViVoServer
{
    public abstract class AbstractServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractServer));
        protected IPAddress addr = IPAddress.Any;
        private int port;

        public AbstractServer() { 
        }

        public AbstractServer(int port) {
            this.port = port;
            log.Info("Creating server");
        }

        public virtual void runServer() {
            log.Info("Running server");
        }
        public virtual void handleClient(Object client) {
            log.Info("Handle client in Abstract");
        }
    }
}
