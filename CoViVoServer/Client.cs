using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoViVoServer
{
    public class ClientList : List<Client> {
        public bool delete(Client client) {
            lock (this)
            {
                return this.Remove(client);
            }
        }
        public void add(Client client) {
            lock (this) {
                this.Add(client);
            }
        }
    }
    public class Client
    {
        public String name { get; set; }
        public IPEndPoint udpAddress { get; set; }
        public long lastAction { get; set; }
        private List<Channel> channels;

        public Client(String name) {
            this.name = name;
            udpAddress = null;
            lastAction = Utils.currentTimeInMillis();
            channels = new List<Channel>();
        }

        public override string ToString()
        {
            return "---name: " + name + " add: " + udpAddress + " action: " + lastAction;
        }

        public override bool Equals(object obj)
        {
            Client temp = (Client)obj;
            return name.Equals(temp.name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
