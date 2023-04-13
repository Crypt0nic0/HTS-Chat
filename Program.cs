using System.Net;
using System.Net.Sockets;
using System.Text;
using Chat;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 2345);

List<Client> clients = new List<Client>();


try
{
    socket.Bind(endpoint);
    socket.Listen();
}
catch
{
    Console.WriteLine("Impossible de démarrer le serveur");
    Environment.Exit(-1);
}

try
{
    while (true)
    {
        var socketcli = socket.Accept();
        if (socketcli.RemoteEndPoint is not null)
        {
            Console.WriteLine("Client connecté depuis l'adresse : " + socketcli.RemoteEndPoint.ToString());
            var client = new Client
            {
                Socket = socketcli,
                Id = Guid.NewGuid()
            };
            clients.Add(client);

            Thread t = new Thread(EcouterClient);
            t.IsBackground = true;
            t.Start(client);
        }
    }
}
catch
{
    Console.WriteLine("La communication avec le client est impossible.");
}
finally
{
    if (socket.Connected)
    {
        socket.Shutdown(SocketShutdown.Both);
    }
    socket.Close();
}

void EcouterClient(Object? obj)
{
    if (obj is Client client)
    {
        try
        {
            while (string.IsNullOrWhiteSpace(client.nom))
            {
                var message = "Veuillez saisir votre nom";
                byte[] buff = Encoding.UTF8.GetBytes(message);
                client.Socket.Send(buff);

                byte[] nomBuff = new byte[128];
                int read = client.Socket.Receive(nomBuff);
                client.nom = Encoding.UTF8.GetString(nomBuff, 0, read);
            }
            while (true)
            {
                byte[] buffer = new byte[4096];
                int nb = client.Socket.Receive(buffer);
                var message = Encoding.UTF8.GetString(buffer, 0, nb);
                Console.WriteLine($"Message reçu de {client.nom} : {message}");

                byte[] sendBuffer = new byte[8192];
                sendBuffer = Encoding.UTF8.GetBytes($"{client.nom} : {message}");
                foreach (var c in clients)
                {
                    try
                    {
                        if (c.Id != client.Id)
                        {
                            c.Socket.Send(sendBuffer);
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Le client {client.nom} s'est déconnecté");
                    }
                }
            }
        }
        catch
        {
            Console.WriteLine($"Le client {client.nom} s'est déconnecté");
        }
        finally
        {
            if (client.Socket.Connected)
            {
                client.Socket.Shutdown(SocketShutdown.Both);
            }
            client.Socket.Close();
            clients.Remove(client);
        }
    }
}
