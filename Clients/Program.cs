using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
class Program
{
    static void Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.1.216"), 2000);
        try
        {
            socket.Connect(ip);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Connection was Successful ..");
            Console.ResetColor();
            Thread th = new Thread(() => ListenAndPrint(socket));
            th.Start();
            ReadMessagefromConsole(socket);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
    static void ReadMessagefromConsole(Socket socket)
    {
        Console.WriteLine("Please enter your name :");
        string name = Console.ReadLine();
        Console.BackgroundColor= ConsoleColor.DarkRed;
        Console.WriteLine($"Hello {name} ' entet 'exit' to Exit '");
        Console.ResetColor();
        while (true)
        {
            string msg = Console.ReadLine();
            if (msg == "exit")
                break;

            string message = $"{name}:{msg}";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            socket.Send(buffer);
        }
    }
    static void ListenAndPrint(Socket socket)
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (true)
            {
                int size = socket.Receive(buffer);
                if (size > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, size);
                    Console.WriteLine($"{message}");
                }
                if (size == 0)
                    break;
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}