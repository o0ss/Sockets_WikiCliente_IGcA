using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Cliente
{
    class Program
    {
        public const int MAX_BYTES = 1024;
        static void Main(string[] args)
        {
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
                string text_to_send = "", text_recvd = "";

                global::System.Console.WriteLine("Ingrese texto para enviar al Servidor: ");

                while (text_to_send != "exit")
                {
                    text_to_send = Console.ReadLine();
                    bytes_to_send = Encoding.ASCII.GetBytes(text_to_send + "<EOF>");
                    sender.Send(bytes_to_send);

                    num_bytes_recvd = sender.Receive(bytes_recvd);
                    text_recvd = Encoding.ASCII.GetString(bytes_recvd, 0, num_bytes_recvd);
                    global::System.Console.WriteLine("Servidor: " + text_recvd);
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