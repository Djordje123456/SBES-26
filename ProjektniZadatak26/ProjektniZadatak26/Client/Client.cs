using AuditManager;
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
    public class Client : ChannelFactory<IDatabaseManagement>, IDatabaseManagement
    {
        IDatabaseManagement factory;

        public Client(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void CreateDatabase()
        {
            try
            {
                factory.CreateDatabase();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        
        public void ArchiveDatabase()
        {
            try
            {
                factory.ArchiveDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        public void DeleteDatabase()
        {
            try
            {
                factory.DeleteDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        public void AddEntry(DatabaseEntry entry)
        {
            try
            {
                factory.AddEntry(entry);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        public void ModifyEntry(DatabaseEntry entry)
        {
            try
            {
                factory.ModifyEntry(entry);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        public double AvgCityConsumption(string city)
        {
            double res = -1;
            try
            {
                res = factory.AvgCityConsumption(city);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return res;
        }

        public double AvgRegionConsumption(string region)
        {
            double res = -1;
            try
            {
                res = factory.AvgRegionConsumption(region);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return res;
        }

        public DatabaseEntry HighestRegionConsumer(string region)
        {
            DatabaseEntry res = null;
            try
            {
                res = factory.HighestRegionConsumer(region);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return res;
        }
    }
}
