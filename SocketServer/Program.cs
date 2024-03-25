using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
class Program
{
    static Socket[] numofClient = new Socket[100];
    static List<string> messegebeforconnect = new List<string>();
    static int cnt = 0; // count accepted client ;
    static void Main(string[] args)
    {
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ip  = new IPEndPoint(IPAddress.Any, 2000);
        server.Bind(ip);//cheak the ip and protocol good to use
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("The server has been created successfully...");
        Console.ResetColor();
        server.Listen(100);
        AcceptedConnections(server);
        server.Shutdown(SocketShutdown.Both);
        server.Close();
    }
    static void AcceptedConnections(Socket server)
    {
        while (true)
        {
            Socket client = server.Accept();
            numofClient[cnt++] = client;
            foreach (string i in messegebeforconnect)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(i);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine( client.Send(buffer));
            }
            Console.ResetColor();
            Thread thread = new Thread(() => PrintDate(client));
            thread.Start();
        }
    }
    static void PrintDate(Socket socket)
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            int size = socket.Receive(buffer);
            if (size > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, size);// convert byte to string 
                Console.WriteLine(message);
                SendMessageToClients(message, socket);
                messegebeforconnect.Add(message);
            }
            if (size == 0)
                break;
        }
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
    static void SendMessageToClients(string message, Socket usersender)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        foreach (Socket i in numofClient)// send message to user connecting only
        {
            if (i != null && i != usersender)
            {
                i.Send(buffer);
            }
        }
    }
}
