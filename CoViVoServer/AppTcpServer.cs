using System;
using System.Collections.Concurrent;
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
        public AppTcpServer(ClientList clients, ConcurrentDictionary<string, Channel> channels)
            : base(clients, channels, Consts.STANDARD_TCP_PORT_RCV)
        {
        }

        public void currentUserList() {
            log.Info("User list:");
            foreach (Client user in clients) {
                log.Info(user);
            }
        }

        public override void handleClient(Object client)
        {
            base.handleClient(client);
            TcpClient tcpClient = (TcpClient)client;
            Message message = receiveMessage(tcpClient);
            string userName = message.user;
            log.Info("Received message: " + message.GetType().Name + " from: " + message.user);
            Client requestClient = clients.findClient(message.user);
            if (message is JoinServer) {
                if (requestClient == null)
                {
                    requestClient = new Client(message.user);
                    clients.Add(requestClient);
                }
                else {
                    log.Info("Client with name: " + requestClient.name + " already exists");
                }
                currentUserList();
                log.Info(requestClient + " has joined the server");
            }
            else if (message is LeaveServer) {
                eraseUser(requestClient);
                currentUserList();
                log.Info(requestClient + " has left the server");
            }
            else if (message is StartChannel) {
                StartChannel startChannel = (StartChannel)message;
                string channelName = startChannel.channelName;
                channels[channelName] = new Channel(channelName);
                log.Info("Starting channel: " + channelName + " by: " + requestClient);
            }
            else if (message is JoinChannel) {
                JoinChannel joinChannel = (JoinChannel)message;
                string channelName = joinChannel.channelName;
                channels[channelName].listeners.Add(requestClient);
                log.Info("Join channel: " + channelName + " by: " + requestClient);
            }
            else if (message is LeaveChannel) {
                LeaveChannel leaveChannel = (LeaveChannel)message;
                string channelName = leaveChannel.channelName;
                channels[channelName].listeners.Remove(requestClient);
                log.Info("Join channel: " + channelName + " by: " + requestClient);
            }
            else if (message is RequestChannelList) { 
                ChannelList channelList = new ChannelList();
                channelList.channelList = channels.Keys.ToArray();
                sendMessage(tcpClient, channelList);
                log.Info("Answering channel list");
            }
            tcpClient.Close();
        }
    }
}
