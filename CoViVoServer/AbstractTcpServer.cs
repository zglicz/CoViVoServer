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
    public abstract class AbstractTcpServer : AbstractServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractTcpServer));
        protected TcpListener tcpListener;

        protected AbstractTcpServer() : this(new ClientList()) { 
        }

        protected AbstractTcpServer(ClientList clients)
            : this(clients, Consts.STANDARD_TCP_PORT)
        {
        }

        protected AbstractTcpServer(ClientList clients, int port)
            : base(clients)
        {
            this.tcpListener = new TcpListener(addr, port);
        }

        public override void runServer()
        {
            base.runServer();
            this.tcpListener.Start();
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                handleClient(client);
            }
        }

        public void sendMessage(Object client, Message message)
        {
            log.Info("Sending message : " + message.GetType().Name);
            byte[] messageWrapped = Util.Wrap(message);
            NetworkStream networkStream = ((TcpClient)client).GetStream();
            networkStream.Write(messageWrapped, 0, messageWrapped.Length);
        }

        public Message receiveMessage(TcpClient client) {
            log.Info("Receive message");
            NetworkStream networkStream = client.GetStream();
            byte[] wrappedMessage = new byte[Consts.BUFFER_SIZE];
            networkStream.Read(wrappedMessage, 0, Consts.BUFFER_SIZE);
            return Util.Unwrap(wrappedMessage);
        }
    }
}
