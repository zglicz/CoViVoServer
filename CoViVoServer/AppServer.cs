﻿using System;
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
    public class AppServer : AbstractServer {

        private static readonly ILog log = LogManager.GetLogger(typeof(AppServer));
        List<String> users;

        public AppServer() : base() {
            users = new List<string>();
        }

        public void currentUserList() {
            log.Info("User list:");
            foreach (String user in users) {
                log.Info("-- " + user);
            }
        }

        public override void handleClient(TcpClient tcpClient)
        {
            byte[] array = new byte[1024];
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.Read(array, 0, 1024);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Console.WriteLine(encoding.GetString(array));
            Message msg = Util.Unwrap(array);
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

    public class ChannelHandler {
        private int port;
        private String sourceIpAddress;
        private IPEndPoint ipEndPoint;
        private Socket socket;

        List<EndPoint> clients;

        public ChannelHandler(int portNumber) {
            this.port = portNumber;
            ipEndPoint = new IPEndPoint(IPAddress.Any, this.port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(ipEndPoint);
        }

        public void runChannel(Object data) {
            sourceIpAddress = (String)data;
            while (true) {
                // odczytanie komunikatu
            }
        }
    }
}
