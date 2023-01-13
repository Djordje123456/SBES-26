using CertificateManager;
using Contracts;
using SecurityManager.cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackupService
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "wcfservice";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9998/BackupService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            using (BackupService proxy = new BackupService(binding, address))
            {
                List<DatabaseEntry> entryList = new List<DatabaseEntry>();
                while (true)
                {
                    Thread.Sleep(30000);
                    entryList = proxy.PullDatabase();
                    SaveDatabase(entryList);
                }
            }
        }

        private static void SaveDatabase(List<DatabaseEntry> entryList)
        {
            string databasePath = @"..\..\BackupDatabase.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));

            using (var stream = File.Create(databasePath))
            {
                serializer.Serialize(stream, entryList);
                Console.WriteLine("Backup saved.");
            }
        }
    }
}
