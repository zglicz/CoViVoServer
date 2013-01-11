using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoViVoServer
{
    public class BasicTcpServer : AbstractTcpServer
    {
        /// <summary>
        ///     Oczekuje na polaczenie od klienta i jest prosta konwersacja
        /// </summary>
        /// <param name="client"></param>
        public override void handleClient(Object client)
        {
            base.handleClient(client);
            TcpClient tcpClient = (TcpClient)client;
            int recv;
            byte[] data = new byte[Consts.BUFFER_SIZE];
            NetworkStream networkStream = tcpClient.GetStream();
            recv = networkStream.Read(data, 0, 1024);
            Console.WriteLine(" read : " + data);
            String message = "Hello to server";
            data = ASCIIEncoding.UTF8.GetBytes(message);
            networkStream.Write(data, 0, data.Length);
            tcpClient.Close();
        }
    }
}
