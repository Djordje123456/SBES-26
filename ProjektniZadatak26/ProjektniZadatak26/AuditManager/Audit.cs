using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditManager
{
    public class Audit
    {
        private static EventLog customLog = null;
        const string SourceName = "AuditManager.Audit";
        const string LogName = "DatabaseManagement";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void AuthentificationSuccess(string userName)
        {
            if (customLog != null)
            {
                string AuthentificationSuccess = AuditEvents.AuthentificationSuccess;
                string message = String.Format(AuthentificationSuccess, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthentificationSuccess));
            }
        }

        public static void AuthentificationFailed(string userName)
        {
            if (customLog != null)
            {
                string AuthentificationFailed = AuditEvents.AuthentificationFailed;
                string message = String.Format(AuthentificationFailed, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthentificationFailed));
            }
        }
        
        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            if (customLog != null)
            {
                string AuthorizationSuccess = AuditEvents.AuthorizationSuccess;
                string message = String.Format(AuthorizationSuccess, userName, serviceName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AuthorizationSuccess));
            }
        }

        public static void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string AuthorizationFailed = AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationFailed, userName, serviceName, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AuthorizationFailed));
            }
        }
        //
        public static void CreateDatabaseSuccess()
        {
            if (customLog != null)
            {
                string CreateDatabaseSuccess = AuditEvents.CreateDatabaseSuccess;
                string message = String.Format(CreateDatabaseSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.CreateDatabaseSuccess));
            }
        }
        public static void CreateDatabaseFailed()
        {
            if (customLog != null)
            {
                string CreateDatabaseFailed = AuditEvents.CreateDatabaseFailed;
                string message = String.Format(CreateDatabaseFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.CreateDatabaseFailed));
            }
        }
        public static void ArchiveDatabaseSuccess()
        {
            if (customLog != null)
            {
                string ArchiveDatabaseSuccess = AuditEvents.ArchiveDatabaseSuccess;
                string message = String.Format(ArchiveDatabaseSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ArchiveDatabaseSuccess));
            }
        }
        public static void ArchiveDatabaseFailed()
        {
            if (customLog != null)
            {
                string ArchiveDatabaseFailed = AuditEvents.ArchiveDatabaseFailed;
                string message = String.Format(ArchiveDatabaseFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ArchiveDatabaseFailed));
            }
        }
        public static void DeleteDatabaseSuccess()
        {
            if (customLog != null)
            {
                string DeleteDatabaseSuccess = AuditEvents.DeleteDatabaseSuccess;
                string message = String.Format(DeleteDatabaseSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.DeleteDatabaseSuccess));
            }
        }
        public static void DeleteDatabaseFailed()
        {
            if (customLog != null)
            {
                string DeleteDatabaseFailed = AuditEvents.DeleteDatabaseFailed;
                string message = String.Format(DeleteDatabaseFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.DeleteDatabaseFailed));
            }
        }
        public static void AddEntrySuccess()
        {
            if (customLog != null)
            {
                string AddEntrySuccess = AuditEvents.AddEntrySuccess;
                string message = String.Format(AddEntrySuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AddEntrySuccess));
            }
        }
        public static void AddEntryFailed()
        {
            if (customLog != null)
            {
                string AddEntryFailed = AuditEvents.AddEntryFailed;
                string message = String.Format(AddEntryFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AddEntryFailed));
            }
        }
        public static void ModifyEntrySuccess()
        {
            if (customLog != null)
            {
                string ModifyEntrySuccess = AuditEvents.ModifyEntrySuccess;
                string message = String.Format(ModifyEntrySuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ModifyEntrySuccess));
            }
        }
        public static void ModifyEntryFailed()
        {
            if (customLog != null)
            {
                string ModifyEntryFailed = AuditEvents.ModifyEntryFailed;
                string message = String.Format(ModifyEntryFailed);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ModifyEntryFailed));
            }
        }
    }
}
