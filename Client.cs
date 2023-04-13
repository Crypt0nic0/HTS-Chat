using System;
using System.Net.Sockets;

namespace Chat
{
    public class Client
    {
        public Socket Socket { get; set; }
        public string nom { get; set; }

        public Guid Id { get; set; }
    }
}
