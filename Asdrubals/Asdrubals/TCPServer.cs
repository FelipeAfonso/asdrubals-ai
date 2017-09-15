using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Asdrubals {
    class TCPServer {

        private static IPAddress ipAd;
        private static TcpListener server;
        public static List<TcpClient> clients = new List<TcpClient>();
        public static bool InUse = false;

        public static void start() {
            ipAd = IPAddress.Parse(TCPServer.GetLocalIPAddress());
            //ipAd = IPAddress.Parse("10.0.0.170");
            //var ip = new WebClient().DownloadString("http://icanhazip.com");
            //ipAd = IPAddress.Parse(ip.Substring(0,ip.Length-1));
            server = new TcpListener(ipAd, 1209);
            clients.ForEach(c => c = default(TcpClient));
            try {
                server.Start();
                Console.WriteLine("Servidor ouvindo na porta 1209 com sucesso, IP: " + ipAd.ToString());
            } catch (Exception e) {
                Console.WriteLine("Falha ao iniciar servidor: " + e.Message);
            }
        }

        public static bool accept() {
            try {
                var client = server.AcceptTcpClient();
                if(!clients.Contains(client)) clients.Add(client);
                Console.WriteLine("Cliente conectado com sucesso");
                return true;
            } catch { return false; }
        }
        

        public static string receive(TcpClient client) {
            byte[] b = new byte[256];
            var encoder = new ASCIIEncoding();
            client.Client.Receive(b);
            
            return Encoding.UTF8.GetString(b).Substring(0, Encoding.UTF8.GetString(b).IndexOf("\r"));
        }

        //public static bool send(String message) { try { var encoder = new ASCIIEncoding(); client.Client.Send(encoder.GetBytes(message)); return true; } catch { return false; } }
        public static bool send(String message, TcpClient client) {
            try {
                var encoder = new ASCIIEncoding();
                client.Client.Send(encoder.GetBytes(message));
                return true;
            } catch {
                return false;
            }
        }
        public static bool send(byte[] message, TcpClient client) {
            try {
                client.Client.Send(message);
                return true;
            } catch {
                return false;
            }
        }
        public static void close() { /*client.Close();*/ server.Stop(); }
        public static void closeClient(TcpClient client) { clients.Remove(client); client.Close(); }
        //public static bool isOcuppied() { return inUse; }

        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
