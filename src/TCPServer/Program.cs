using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StartTCPServer();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async static Task StartTCPServer()
        {
            Console.WriteLine("Starting TCP Server.");

            //IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("192.168.29.88");
            //IPAddress ipAddress = ipHostInfo.AddressList[0];

            IPEndPoint ipEndPoint = new(IPAddress.Parse("192.168.29.88"), 9693);

            using Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(100);

            Console.WriteLine($"Started TCP Server at {ipEndPoint}.");

            var handler = await listener.AcceptAsync();
            
            Console.WriteLine($"Client connected from {handler.RemoteEndPoint}");

            while (true)
            {
                // Receive message.
                var buffer = new byte[1_024];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);

                var response = Encoding.UTF8.GetString(buffer, 0, received);

                Console.WriteLine($"Msg Received: {response}");

                var echoBytes = Encoding.UTF8.GetBytes($"Eco Back {response}");

                await handler.SendAsync(echoBytes, 0);
            }
        }
    }
}