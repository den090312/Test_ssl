using System;
using System.Diagnostics;

namespace Telarm2
{
    class Program
    {
        static void Main(string[] args)
        {
            Chilkat.Global glob = new Chilkat.Global();
            bool success = glob.UnlockBundle("Anything for 30-day trial");
            if (success != true)
            {
                Debug.WriteLine(glob.LastErrorText);
                return;
            }

            int status = glob.UnlockStatus;
            if (status == 2)
            {
                Debug.WriteLine("Unlocked using purchased unlock code.");
            }
            else
            {
                Debug.WriteLine("Unlocked in trial mode.");
            }

            // The LastErrorText can be examined in the success case to see if it was unlocked in
            // trial more, or with a purchased unlock code.
            Debug.WriteLine(glob.LastErrorText);

            var certPath = $@"{AppDomain.CurrentDomain.BaseDirectory}server_20110101.pfx";

            var server = "83.220.246.39";
            var port = 9000;
            var maxWaitMillisec = 10000;
            //var certPass = "qZxxoGdCsJhL20Z+";

            using (var socket = new Chilkat.Socket
            {
                SocksHostname = server,
                SocksPort = port,
                SocksUsername = "22600062",
                SocksPassword = "444444",
                SocksVersion = 5,
                MaxReadIdleMs = maxWaitMillisec,
                MaxSendIdleMs = maxWaitMillisec
            })
            {
                var cert = new Chilkat.Cert();
                cert.LoadFromFile(certPath);

                socket.SetSslClientCert(cert);

                socket.Connect(server, port, true, maxWaitMillisec);

                var receivedString = socket.ReceiveString();

                Console.WriteLine(receivedString ?? "none");
            }
        }
    }
}
