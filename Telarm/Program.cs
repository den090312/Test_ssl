using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Telarm
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            //var certificate = GetCertificatePEM();

            var certificate = GetCertificatePFX();

            var server = "83.220.246.39";
            var port = 9000;

            using (var tcpClient = new TcpClient(server, port))
            {
                using (var sslStream = new SslStream(tcpClient.GetStream()))
                {
                    var protocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Ssl2 | SslProtocols.Ssl3;

                    //sslStream.AuthenticateAsServer(certificate, true, protocols, false);

                    sslStream.AuthenticateAsServer(certificate);
                }

                Console.WriteLine("CONNECTED");
            }

            //IPHostEntry host = Dns.GetHostEntry("localhost");
            //IPAddress ipAddress = host.AddressList[0];
            //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            //// Create a TCP/IP  socket.    
            //Socket sender = new Socket(ipAddress.AddressFamily,
            //    SocketType.Stream, ProtocolType.Tcp);

            //var socket = new Socket();
            //socket.

            //using (var networkStream = new NetworkStream()
            //{

            //}
        }

        private static X509Certificate2 GetCertificatePEM()
        {
            var certPath = $@"{AppDomain.CurrentDomain.BaseDirectory}server_20110101.pem";
            var pem = File.ReadAllText(certPath);

            var certBuffer = Crypto.GetBytesFromPEM(pem, "CERTIFICATE");
            var certificate = new X509Certificate2(certBuffer);

            var keyBuffer = Crypto.GetBytesFromPEM(pem, "RSA PRIVATE KEY");
            var rsaPrivateKey = Crypto.DecodeRsaPrivateKey(keyBuffer);

            certificate.PrivateKey = rsaPrivateKey;

            return certificate;
        }

        private static X509Certificate2 GetCertificatePFX()
        {
            var certPath = $@"{AppDomain.CurrentDomain.BaseDirectory}server_20110101.pfx";
            //var pem = File.ReadAllText(certPath);

            //var certBuffer = Crypto.GetBytesFromPEM(pem, "CERTIFICATE");
            //var certificate = new X509Certificate2(certBuffer);

            //var keyBuffer = Crypto.GetBytesFromPEM(pem, "RSA PRIVATE KEY");
            //var rsaPrivateKey = Crypto.DecodeRsaPrivateKey(keyBuffer);

            //certificate.PrivateKey = rsaPrivateKey;

            var certificate = new X509Certificate2(X509Certificate.CreateFromSignedFile(certPath));

            return certificate;
        }
    }
}


