using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoViVoServer
{
    class TestProgram {
        public static void glownySposob() {
            ClientList clients = new ClientList();
            ConcurrentDictionary<string, Channel> channels = new ConcurrentDictionary<string, Channel>();
            AbstractServer udpServer = new AppUdpServer(clients, channels);
            AbstractServer tcpServer = new AppTcpServer(clients, channels);

            Thread udpThread = new Thread(new ThreadStart(udpServer.runServer));
            Thread tcpThread = new Thread(new ThreadStart(tcpServer.runServer));

            udpThread.Start();
            tcpThread.Start();
        }

        public static void basicTcp() {
            AbstractServer basicTcpServer = new BasicTcpServer();
            basicTcpServer.runServer();
        }

        public static void basicUdp() {
            AbstractUdpServer basicUdpServer = new BasicUdpServer();
            basicUdpServer.runServer();
        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            glownySposob();
        }
    }
}
