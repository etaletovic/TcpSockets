using Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer();
        }

        static void RunServer()
        {
            var ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            var localEndpoint = new IPEndPoint(ipAddress, Constants.Port);

            var serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(localEndpoint);

                serverSocket.Listen(Constants.MaxClients);

                while (true)
                {
                    Console.WriteLine($"Waiting connection on {ipAddress}:{Constants.Port}...");

                    var clientSocket = serverSocket.Accept();

                    var buffer = new byte[Constants.BufferByteSize];

                    string data = default;

                    while (true)
                    {
                        var messageLength = clientSocket.Receive(buffer);

                        data += Encoding.ASCII.GetString(buffer, 0, messageLength);

                        if (data.IndexOf("<EOF>") > -1) break;
                    }

                    Console.WriteLine("Text received: {0} ", data);

                    var responseToClient = Encoding.ASCII.GetBytes("Got it! --Server");
                    clientSocket.Send(responseToClient);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}