using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Cliente
{
    class Program
    {
        public const int MAX_BYTES = 10240;
        static void Main(string[] args)
        {
            System.Console.Title = "Cliente";
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip_addr = host.AddressList[0];
            IPEndPoint remote_ep = new IPEndPoint(ip_addr, 11200);
            try
            {
                global::System.Console.WriteLine("Conectando con el Servidor...");
                Socket sender = new Socket(ip_addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remote_ep);

                byte[] bytes_to_send, bytes_recvd = new byte[MAX_BYTES];
                int num_bytes_recvd;
                string query = "", text_recvd = "";

                while(true)
                {
                    Console.Write("\n\n\nBuscar en Wikipedia: ");
                    query = Console.ReadLine();

                    if (query == "exit") break;

                    bytes_to_send = Encoding.ASCII.GetBytes(query + "<EOF>");
                    sender.Send(bytes_to_send);

                    num_bytes_recvd = sender.Receive(bytes_recvd);
                    text_recvd = Encoding.UTF8.GetString(bytes_recvd, 0, num_bytes_recvd);
                    global::System.Console.WriteLine("\n\n\nServidor: \n\n" + text_recvd);
                }
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}