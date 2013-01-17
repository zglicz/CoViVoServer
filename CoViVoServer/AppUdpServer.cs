using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using WrapperLib;

namespace CoViVoServer
{
    class AppUdpServer : AbstractUdpServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AppUdpServer));
        public AppUdpServer(ClientList clients)
            : base(clients, Consts.STANDARD_UDP_PORT_RCV, Consts.STANDARD_UDP_PORT_SEND)
        {
        }

        public override void handleClient(object client)
        {
            base.handleClient(client);
            Tuple<IPEndPoint, Message> tuple = (Tuple<IPEndPoint, Message>)client;
            IPEndPoint clientAddress = tuple.Item1;
            Message message = tuple.Item2;
            Client recClient = new Client(message.user);
            log.Info("Received message: " + message.GetType().Name + " from: " + message.user);
            if (message is Alive) {
                clients.printClients();
                int x = clients.findClient(recClient);
                if (x == -1)
                {
                    log.Debug("client not found");
                }
                else
                {
                    clients[x].lastAction = Utils.currentTimeInMillis();
                    clients[x].udpAddress = clientAddress;
                    log.Info("Alive from: " + message.user);
                }
            }
            else if (message is ChannelData) { 
                ChannelData channelMsg = (ChannelData)message;
                string channelName = channelMsg.channelName;
                log.Info("sending on channel: " + channelName);
                printChannels();
                Channel channel = channels[channelName];
                log.Info("Broadcasting");
                broadcast(channel, channelMsg);
            }
        }

        public override void runServer()
        {
            base.runServer();
            //Thread check = new Thread(new ThreadStart(checkAlive));
            //check.Start();
            while (true)
            {
                IPEndPoint client = new IPEndPoint(addr, 0);
                byte[] messageWrapped = listener.Receive(ref client);
                Message message = Util.Unwrap(messageWrapped);
                handleClient(new Tuple<IPEndPoint, Message>(client, message));
            }
        }

        private void broadcast(Channel channel, ChannelData channelData) { 
            foreach (Client client in channel.listeners) {
                if (client.udpAddress != null)
                {
                    sendMessage(client.udpAddress, channelData);
                }
                else {
                    log.Debug("No address for: " + client);
                }
            }
        }
        /*
        public void checkAlive()
        {
            while (true)
            {
                long curTime = Utils.currentTimeInMillis();
                foreach (Client client in clients)
                {
                    long diff = curTime - client.lastAction;
                    if (diff > Consts.REQUEST_TIME)
                    {
                        eraseUser(client);
                    }
                }
                System.Threading.Thread.Sleep(Consts.REQUEST_TIME);
            }
        }*/
    }
}
