using Contracts;
using SecurityManager.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            Console.WriteLine(name);

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";

            //Windows Authentification
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            string str;
            DatabaseEntry entry;
            double res;

            using (Client proxy = new Client(binding, new EndpointAddress(new Uri(address))))
            {
                while (true)
                {
                    Console.WriteLine("0 - Create Database");
                    Console.WriteLine("1 - Archive Database");
                    Console.WriteLine("2 - Delete Database");
                    Console.WriteLine("3 - Add Entry to Database");
                    Console.WriteLine("4 - Modify Entry in Database");
                    Console.WriteLine("5 - Get Average City Consumption");
                    Console.WriteLine("6 - Get Average Region Consumption");
                    Console.WriteLine("7 - Get Highest Region Consumer");

                    str = Console.ReadLine();
                    Console.WriteLine();

                    switch (str)
                    {
                        case "0":
                            proxy.CreateDatabase();
                            break;
                        case "1":
                            proxy.ArchiveDatabase();
                            break;
                        case "2":
                            proxy.DeleteDatabase();
                            break;
                        case "3":
                            entry = InputEntry();
                            proxy.AddEntry(entry);
                            break;
                        case "4":
                            Console.Write("Enter ID of the Entry you want to modify: ");
                            int id = int.Parse(Console.ReadLine());
                            entry = InputEntry();
                            entry.Id = id; 
                            proxy.ModifyEntry(entry);
                            break;
                        case "5":
                            Console.Write("Enter a City for which to show average yearly consumption: ");
                            str = Console.ReadLine();
                            res = proxy.AvgCityConsumption(str);
                            if(res == -1)
                            {
                                Console.WriteLine("No city with that name exists in the Database.");
                            }
                            else
                            { 
                                Console.WriteLine("Average consumption for the city of {0} is {1}MWh", str, res);
                            }
                            break;
                        case "6":
                            Console.Write("Enter a Region for which to show average yearly consumption: ");
                            str = Console.ReadLine();
                            res = proxy.AvgRegionConsumption(str);
                            if (res == -1)
                            {
                                Console.WriteLine("No region with that name exists in the Database.");
                            }
                            else
                            {
                                Console.WriteLine("Average consumption for the region of {0} is {1}MWh", str, res);
                            }
                            break;
                        case "7":
                            Console.Write("Enter a Region for which to show the highest consumer: ");
                            str = Console.ReadLine();
                            entry = proxy.HighestRegionConsumer(str);
                            if (entry == null)
                            {
                                Console.WriteLine("No data for cities in region with that name.");
                            }
                            else
                            {
                                Console.WriteLine("Highest consumer for the region of {0} is:", str);
                                Console.WriteLine("ID: " + entry.Id);
                                Console.WriteLine("City: " + entry.City);
                                Console.WriteLine("Year: " + entry.Year);
                                Console.WriteLine("Yearly consumption: {0}MWh", entry.GetYearlyConsumption());
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static DatabaseEntry InputEntry()
        {
            double[] consumption = new double[12];
            Console.WriteLine();
            Console.WriteLine("Enter entry data: ");
            Console.WriteLine();

            Console.Write("Region: ");
            string region = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            Console.Write("Year: ");
            string year = Console.ReadLine();

            Console.WriteLine("Consumption for each month:");
            Console.Write("Jan: ");
            consumption[0] = double.Parse(Console.ReadLine());
            Console.Write("Feb: ");
            consumption[1] = double.Parse(Console.ReadLine());
            Console.Write("Mar: ");
            consumption[2] = double.Parse(Console.ReadLine());
            Console.Write("Apr: ");
            consumption[3] = double.Parse(Console.ReadLine());
            Console.Write("May: ");
            consumption[4] = double.Parse(Console.ReadLine());
            Console.Write("Jun: ");
            consumption[5] = double.Parse(Console.ReadLine());
            Console.Write("Jul: ");
            consumption[6] = double.Parse(Console.ReadLine());
            Console.Write("Aug: ");
            consumption[7] = double.Parse(Console.ReadLine());
            Console.Write("Sep: ");
            consumption[8] = double.Parse(Console.ReadLine());
            Console.Write("Oct: ");
            consumption[9] = double.Parse(Console.ReadLine());
            Console.Write("Nov: ");
            consumption[10] = double.Parse(Console.ReadLine());
            Console.Write("Dec: ");
            consumption[11] = double.Parse(Console.ReadLine());

            return new DatabaseEntry(1, region, city, year, consumption);
        }
    }
}
