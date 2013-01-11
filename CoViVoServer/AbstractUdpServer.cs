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
    class AbstractUdpServer : AbstractServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractUdpServer));
        protected UdpClient serverUdp;

        protected AbstractUdpServer()
            : this(Consts.STANDARD_UDP_PORT)
        {
        }


        protected AbstractUdpServer(int port)
            : base(port)
        {
            this.serverUdp = new UdpClient(new IPEndPoint(addr, port));
        }

        public override void runServer()
        {
            base.runServer();
            while (true)
            {
                IPEndPoint client = new IPEndPoint(addr, 0);
                byte[] messageWrapped = serverUdp.Receive(ref client);
                Message message = Util.Unwrap(messageWrapped);
                handleClient(new Tuple<IPEndPoint, Message>(client, message));
            }
        }

        public void sendMessage(IPEndPoint ipEndPoint, Message message) {
            log.Info("Send message to: " + ipEndPoint.ToString());
            byte[] messageWrapped = Util.Wrap(message);
            serverUdp.Send(messageWrapped, messageWrapped.Length, ipEndPoint);
        }
    }
}
