﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WrapperLib;

namespace CoViVoServer
{
    public class BasicUdpServer : AbstractUdpServer
    {
        /// <summary>
        ///     Przykladowe dzialanie serwera. Czeka na Alive. Odpowiada na nia : RequestAlive
        /// </summary>
        /// <param name="client"></param>
        public override void handleClient(object client)
        {
            base.handleClient(client);
            Tuple<IPEndPoint, Message> tuple = (Tuple<IPEndPoint, Message>)client;
            IPEndPoint clientAddress = tuple.Item1;
            Message message = tuple.Item2;
            if (message is Alive) {
                RequestAlive serverResponse = new RequestAlive();
                serverResponse.parameters = "Witam witam";
                sendMessage(clientAddress, serverResponse);
            }
        }

        public override void runServer()
        {
            base.runServer();
            while (true)
            {
                IPEndPoint client = new IPEndPoint(addr, 0);
                byte[] messageWrapped = listener.Receive(ref client);
                Message message = Util.Unwrap(messageWrapped);
                handleClient(new Tuple<IPEndPoint, Message>(client, message));
            }
        }
    }
}
