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
    public class AbstractUdpServer : AbstractServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractUdpServer));
        protected UdpClient listener;
        protected UdpClient sender;

        public AbstractUdpServer() : this(new ClientList()) {
        }

        public AbstractUdpServer(ClientList clients)
            : this(clients, Consts.STANDARD_UDP_PORT_RCV, Consts.STANDARD_UDP_PORT_SEND)
        {
        }

        public AbstractUdpServer(ClientList clients, int rcv_port, int send_port)
            : base(clients)
        {
            this.listener = new UdpClient(new IPEndPoint(addr, rcv_port));
            this.sender = new UdpClient(new IPEndPoint(addr, send_port));
        }

        public void sendMessage(IPEndPoint ipEndPoint, Message message) {
            log.Info("Send message to: " + ipEndPoint.ToString());
            byte[] messageWrapped = Util.Wrap(message);
            sender.Send(messageWrapped, messageWrapped.Length, ipEndPoint);
        }
    }
}
