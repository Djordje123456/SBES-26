using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Xml.Serialization;
using SecurityManager.cs;
using AuditManager;
using System.ServiceModel;
using System.Security.Principal;

namespace Service
{
    public class Service : IDatabaseManagement, IBackupService
    {
        private string databasePath = @"..\..\Database.xml";
        
        public void CreateDatabase()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (!File.Exists(databasePath))
                {
                    using (var stream = File.Create(databasePath))
                    {
                        Console.WriteLine("Database created.");

                        try
                        {
                            Audit.CreateDatabaseSuccess();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed to create a Database. Database already exists.");

                    try
                    {
                        Audit.CreateDatabaseFailed();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "CreateDatabase method need Administrate permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        public void ArchiveDatabase()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (File.Exists(databasePath))
                {
                    string path = @"..\..\ArchivedDatabases";
                    string[] filePaths = Directory.GetFiles(path);
                    int numFiles = filePaths.Length;
                    numFiles += 1;
                    string filename = @"\ArchivedDatabase" + numFiles + ".xml";
                    string archivedDatabasePath = path + filename;

                    XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
                    List<DatabaseEntry> entryList = new List<DatabaseEntry>();

                    using (FileStream stream = File.OpenRead(databasePath))
                    {
                        try
                        {
                            entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                        }
                        catch (Exception e)
                        {
                            //Avoiding the empty database exception.
                        }
                    }
                    using (var stream = File.Create(archivedDatabasePath))
                    {
                        serializer.Serialize(stream, entryList);
                        File.Delete(databasePath);
                        Console.WriteLine("Database archived.");

                        try
                        {
                            Audit.ArchiveDatabaseSuccess();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cannot archive the Database. Database doesn't exist.");

                    try
                    {
                        Audit.ArchiveDatabaseFailed();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "ArchiveDatabase method need Administrate permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        public void DeleteDatabase()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                    Console.WriteLine("Database deleted.");

                    try
                    {
                        Audit.DeleteDatabaseSuccess();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Cannot delete the Database. It doesn't exist!");

                    try
                    {
                        Audit.DeleteDatabaseFailed();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "DeleteDatabase method need Administrate permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        public void AddEntry(DatabaseEntry entry)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Write"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (File.Exists(databasePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
                    List<DatabaseEntry> entryList = new List<DatabaseEntry>();

                    using (FileStream stream = File.OpenRead(databasePath))
                    {
                        try
                        {
                            entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                        }
                        catch (Exception e)
                        {
                            //Avoiding the empty database exception.
                        }
                    }
                    entry.Id = entryList.Count;
                    entryList.Add(entry);
                    using (var stream = File.Create(databasePath))
                    {
                        serializer.Serialize(stream, entryList);
                        Console.WriteLine("Entry added to database");

                        try
                        {
                            Audit.AddEntrySuccess();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cannot add Entry to the Database. Database doesn't exist.");

                    try
                    {
                        Audit.AddEntryFailed();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "AddEntry method need Write permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        public void ModifyEntry(DatabaseEntry entry)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Write"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (File.Exists(databasePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
                    List<DatabaseEntry> entryList = new List<DatabaseEntry>();
                    DatabaseEntry entryToRemove = null;

                    using (FileStream stream = File.OpenRead(databasePath))
                    {
                        try
                        {
                            entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                        }
                        catch (Exception e)
                        {
                            //Avoiding the empty database exception.
                        }
                    }
                    foreach (DatabaseEntry existingEntry in entryList)
                    {
                        if (existingEntry.Id == entry.Id)
                        {
                            entryToRemove = existingEntry;
                            break;
                        }
                    }
                    if (entryToRemove != null)
                    {
                        entryList.Remove(entryToRemove);
                        try
                        {
                            entryList.Insert(entry.Id, entry);
                        }
                        catch
                        {
                            entryList.Add(entry);
                        }
                        using (var stream = File.Create(databasePath))
                        {
                            serializer.Serialize(stream, entryList);
                            Console.WriteLine("Entry modified.");

                            try
                            {
                                Audit.ModifyEntrySuccess();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot modify Entry. Database doesn't contain given Entry.");

                        try
                        {
                            Audit.ModifyEntryFailed();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cannot add Entry to the Database. Database doesn't exist.");

                    try
                    {
                        Audit.ModifyEntryFailed();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "ModifyEntry method need Write permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        public double AvgCityConsumption(string city)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
                List<DatabaseEntry> entryList = new List<DatabaseEntry>();

                using (FileStream stream = File.OpenRead(databasePath))
                {
                    try
                    {
                        entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                    }
                    catch (Exception e)
                    {
                        //Avoiding the empty database exception.
                    }
                }
                double res = 0;
                int cnt = 0;
                foreach (DatabaseEntry entry in entryList)
                {
                    if (entry.City.Equals(city))
                    {
                        res += entry.GetYearlyConsumption();
                        cnt++;
                    }
                }
                if (res == 0)
                {
                    Console.WriteLine("There are no such Cities in the Database.");
                    return -1;
                }
                return (res / cnt);
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "AvgCityConsumption method need Read permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return -1;
            }
        }
        
        public double AvgRegionConsumption(string region)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
                List<DatabaseEntry> entryList = new List<DatabaseEntry>();

                using (FileStream stream = File.OpenRead(databasePath))
                {
                    try
                    {
                        entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                    }
                    catch (Exception e)
                    {
                        //Avoiding the empty database exception.
                    }
                }
                double res = 0;
                int cnt = 0;
                foreach (DatabaseEntry entry in entryList)
                {
                    if (entry.Region.Equals(region))
                    {
                        res += entry.GetYearlyConsumption();
                        cnt++;
                    }
                }
                if (res == 0)
                {
                    Console.WriteLine("There are no such Regions in the Database.");
                    return -1;
                }
                return (res / cnt);
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "AvgRegionConsumption method need Read permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return -1;
            }
        }
        
        public DatabaseEntry HighestRegionConsumer(string region)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
                List<DatabaseEntry> entryList = new List<DatabaseEntry>();
                DatabaseEntry highestConsumer = null;

                using (FileStream stream = File.OpenRead(databasePath))
                {
                    try
                    {
                        entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                    }
                    catch (Exception e)
                    {
                        //Avoiding the empty database exception.
                    }
                }
                double max = 0;
                foreach (DatabaseEntry entry in entryList)
                {
                    if ((entry.Region.Equals(region)) && (entry.GetYearlyConsumption() > max))
                    {
                        max = entry.GetYearlyConsumption();
                        highestConsumer = entry;
                    }
                }
                if (highestConsumer == null)
                {
                    Console.WriteLine("There are no Consumers in that region.");
                }
                return highestConsumer;
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(WindowsIdentity.GetCurrent().Name),
                        OperationContext.Current.IncomingMessageHeaders.Action, "HighestRegionConsumer method need Read permission.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
        }

        public List<DatabaseEntry> PullDatabase()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DatabaseEntry>));
            List<DatabaseEntry> entryList = new List<DatabaseEntry>();
            if (File.Exists(databasePath))
            {
                using (FileStream stream = File.OpenRead(databasePath))
                {
                    try
                    {
                        entryList = (List<DatabaseEntry>)serializer.Deserialize(stream);
                    }
                    catch (Exception e)
                    {
                        //Avoiding the empty database exception.
                    }
                }
            }
            else
            {
                Console.WriteLine("Nema baze\n");
            }
            return entryList;
        }
    }
}
