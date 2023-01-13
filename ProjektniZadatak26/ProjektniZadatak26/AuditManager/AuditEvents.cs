using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AuditManager
{
    public enum AuditEventTypes
	{
        AuthentificationSuccess = 0,
        AuthentificationFailed = 1,
        AuthorizationSuccess = 2,
        AuthorizationFailed = 3,
        CreateDatabaseSuccess = 4,
        CreateDatabaseFailed = 5,
        ArchiveDatabaseSuccess = 6,
        ArchiveDatabaseFailed = 7,
        DeleteDatabaseSuccess = 8,
        DeleteDatabaseFailed = 9,
        AddEntrySuccess = 10,
        AddEntryFailed = 11,
        ModifyEntrySuccess = 12,
        ModifyEntryFailed = 13
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthentificationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthentificationSuccess.ToString());
            }
        }

        public static string AuthentificationFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthentificationFailed.ToString());
            }
        }
        
        public static string AuthorizationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
        }

        public static string AuthorizationFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
            }
        }

        public static string CreateDatabaseSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateDatabaseSuccess.ToString());
            }
        }
        public static string CreateDatabaseFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CreateDatabaseFailed.ToString());
            }
        }
        public static string ArchiveDatabaseSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ArchiveDatabaseSuccess.ToString());
            }
        }
        public static string ArchiveDatabaseFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ArchiveDatabaseFailed.ToString());
            }
        }
        public static string DeleteDatabaseSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DeleteDatabaseSuccess.ToString());
            }
        }
        public static string DeleteDatabaseFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.DeleteDatabaseFailed.ToString());
            }
        }
        public static string AddEntrySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddEntrySuccess.ToString());
            }
        }
        public static string AddEntryFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddEntryFailed.ToString());
            }
        }
        public static string ModifyEntrySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ModifyEntrySuccess.ToString());
            }
        }
        public static string ModifyEntryFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ModifyEntryFailed.ToString());
            }
        }
    }
}
