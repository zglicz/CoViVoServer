using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;
using WrapperLib;

namespace CoViVoServer
{
    public class AppTcpServer : AbstractTcpServer {

        private static readonly ILog log = LogManager.GetLogger(typeof(AppTcpServer));
        List<String> users;

        public AppTcpServer() : base() {
            users = new List<string>();
        }

        public void currentUserList() {
            log.Info("User list:");
            foreach (String user in users) {
                log.Info("-- " + user);
            }
        }

        public override void handleClient(Object client)
        {
            base.handleClient(client);
            TcpClient tcpClient = (TcpClient)client;
            Message msg = receiveMessage(tcpClient);
            if (msg is JoinServer) {
                JoinServer joinServerMsg = (JoinServer)msg;
                String userName = joinServerMsg.user;
                users.Add(userName);
                log.Info(userName + " has joined the server");
                currentUserList();
            }
            else if (msg is LeaveServer) {
                LeaveServer leaveServerMsg = (LeaveServer)msg;
                String userName = leaveServerMsg.user;
                users.Remove(userName);
                log.Info(userName + " has left the server");
                currentUserList();
            }
            tcpClient.Close();
        }
    }
}
