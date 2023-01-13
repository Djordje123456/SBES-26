using CertificateManager;
using Contracts;
using SecurityManager.cs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            //string name = "wcfservice";
            Console.WriteLine(name);

            NetTcpBinding bindingCL = new NetTcpBinding();
            string addressCL = "net.tcp://localhost:9999/Service";

            //Windows Authentification
            bindingCL.Security.Mode = SecurityMode.Transport;
            bindingCL.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingCL.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost hostCL = new ServiceHost(typeof(Service));
            hostCL.AddServiceEndpoint(typeof(IDatabaseManagement), bindingCL, addressCL);

            //Defining CustomAuthorizationManager as the preffered one.
            hostCL.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            //Defining our principal settings.
            hostCL.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            hostCL.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            hostCL.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            hostCL.Description.Behaviors.Add(newAudit);

            hostCL.Open();

            Console.WriteLine("Service is opened. Press <enter> to finish...");
            NetTcpBinding bindingBS = new NetTcpBinding();
            string addressBS = "net.tcp://localhost:9998/BackupService";

            //Certificate Authentication
            bindingBS.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost hostBS = new ServiceHost(typeof(Service));
            hostBS.AddServiceEndpoint(typeof(IBackupService), bindingBS, addressBS);

            hostBS.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostBS.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            hostBS.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, name);

            ServiceSecurityAuditBehavior newAudit2 = new ServiceSecurityAuditBehavior();
            newAudit2.AuditLogLocation = AuditLogLocation.Application;
            newAudit2.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            hostBS.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            hostBS.Description.Behaviors.Add(newAudit2);

            hostBS.Open();

            Console.WriteLine("BackupService endpoint is opened.");

            Console.ReadLine();

            hostBS.Close();

            hostCL.Close();
        }
    }
}
