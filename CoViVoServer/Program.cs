using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoViVoServer
{
    class TestProgram {
        // tutaj wybierasz jaki typ serwera ma byc uruchomiony
        static AbstractServer usedServer = new AppTcpServer();
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            usedServer.runServer();
        }
    }
}
