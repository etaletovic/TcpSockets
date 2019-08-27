using Common;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RunClient();
        }

        static void RunClient()
        {
            var ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            var endpoint = new IPEndPoint(ipAddress, Constants.Port);

            var socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(endpoint);

                Console.WriteLine($"Socket connected to -> {socket.RemoteEndPoint.ToString()}");

                var message = Encoding.ASCII.GetBytes("Test Client<EOF>");

                var messageByteCount = socket.Send(message);

                var buffer = new byte[Constants.BufferByteSize];

                int responseByteCount = socket.Receive(buffer);

                var response = Encoding.ASCII.GetString(buffer, 0, responseByteCount);

                Console.WriteLine($"Message from server -> {response}");

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}