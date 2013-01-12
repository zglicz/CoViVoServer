using System;
using System.Collections.Concurrent;
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
        protected ClientList clients;
        protected ConcurrentDictionary<string, Channel> channels
        { get; set; }

        public AbstractServer() { 
        }

        public AbstractServer(ClientList clients) {
            this.clients = clients;
            this.channels = new ConcurrentDictionary<string, Channel>();
            log.Info("Creating server");
        }

        public virtual void runServer() {
            log.Info("Running server");
        }
        public virtual void handleClient(Object client) {
            log.Info("Handle client in Abstract");
        }

        public void eraseUser(Client client) {
            //TODO: add erasing from channels
            clients.delete(client);
        }
    }
}
