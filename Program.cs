using System.Net;
using System.Net.Sockets;


try
{
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    IPAddress add = IPAddress.Any;
    IPEndPoint endpoint = new IPEndPoint(add, 2345);

    socket.Bind(endpoint);
    socket.Listen();
    socket.Accept();
    Console.WriteLine("Connexion OK");
}
catch
{
    Console.WriteLine("Erreur lors de la connexion");
}
