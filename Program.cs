using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    IPAddress add = IPAddress.Any;
    IPEndPoint endpoint = new IPEndPoint(add, 2345);

    socket.Bind(endpoint);
    socket.Listen();
    var socketcli = socket.Accept();
    Console.WriteLine("Connexion : OK");
    if (socketcli.RemoteEndPoint is not null)
    {
        Console.WriteLine($"Client connecté depuis l'adresse : {socketcli.RemoteEndPoint.ToString()}");
        byte[] buffer = new byte[128];
        int nb = socketcli.Receive(buffer);
        Console.WriteLine($"Message reçu : {Encoding.ASCII.GetString(buffer, 0, nb)}");
    }
}
catch
{
    Console.WriteLine("Erreur lors de la connexion");
}
