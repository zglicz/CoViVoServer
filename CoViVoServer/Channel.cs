using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoViVoServer
{
    public class Channel
    {
        private string channelName;
        public ClientList listeners
        { get; set; }

        public Channel (string channelName, Client client) {
            this.channelName = channelName;
            listeners = new ClientList();
            listeners.add(client);
        }
    }
}
