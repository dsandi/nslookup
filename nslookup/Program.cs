using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace networktest
{
    internal class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<string> Hosts = new List<string>(new[] { "google.com", "facebook.com", "microsoft.com", "reddit.com", "netflix.com", "stackoverflow.com" });

        private static void Main()
        {
            do
            {
                while (!Console.KeyAvailable)
                {
                    Console.WriteLine("Press ESC to stop");
                    foreach (var host in Hosts)
                    {
                        Nslookup(host);
                    }
                    Thread.Sleep(5000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        public static void Nslookup(string hostname)
        {
            try
            {
                Dns.GetHostEntry(hostname);
            }
            catch (Exception)
            {
                Log.Info($"Error resolving ({hostname}) using dns ({GetDnsAddress()})");
            }
        }

        private static IPAddress GetDnsAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus != OperationalStatus.Up) continue;
                var ipProperties = networkInterface.GetIPProperties();
                var dnsAddresses = ipProperties.DnsAddresses;

                foreach (var dnsAddress in dnsAddresses)
                {
                    return dnsAddress;
                }
            }
            Log.Error("Unable to find DNS Address");

            throw new InvalidOperationException();
        }

    }
}
