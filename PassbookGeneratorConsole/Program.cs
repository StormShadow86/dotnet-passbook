using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;

using NLog;
using NLog.Config;
using NLog.Targets;
using Passbook;
using Passbook.Generator;
using Passbook.Generator.Configuration;
using Passbook.Generator.Exceptions;
using Passbook.Generator.Fields;

namespace PassbookGeneratorConsole
{
    static class Program
    {
        private static ILogger _logger;

        static void Main()
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1 - Create Example Pass");
            Console.WriteLine("2 - Test json template load");
            Console.WriteLine("Enter your choice (1 or 2) and press Enter");
            string answer = Console.ReadLine();
            int choice = 0;
            if (int.TryParse(answer, out choice))
            {
                switch (choice)
                {
                    case 1:
                        CreateExamplePass();
                        break;
                    case 2:
                        TestJsonTemplateLoad();
                        break;
                }
            }
            
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void TestJsonTemplateLoad()
        {
            JsonTemplateProvider provider = new JsonTemplateProvider();
            provider.LoadJsonTemplateHolder(@"D:\inetpub\wwwroot\Bonobo.Git.Server\App_Data\Repositories\dotnet-passbook\templateJson.json");
        }

        private static void CreateExamplePass()
        {
            string passPath = null;

            ConfigureLogger();
            Console.WriteLine("Creating Pass...");
            byte[] passFile = CreatePassPackage();
            if (passFile != null)
            {
                Console.WriteLine("Pass generated.");
                Console.WriteLine("Saving Pass to temp folder...");
                using (var ms = new MemoryStream(passFile))
                {
                    passPath = @"C:\\Temp\\" + System.Guid.NewGuid().ToString() + ".pkpass";
                    using (var fs = new FileStream(passPath, FileMode.Create, FileAccess.Write))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.CopyTo(fs);
                    }
                }
                Console.WriteLine("Pass saved.");
                Console.WriteLine("Sending Pass...");
                SendPass(passPath);
                Console.WriteLine("Pass sended.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Problem creating pass...");
            }
        }

        private static void ConfigureLogger()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            consoleTarget.Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/file.txt";
            fileTarget.Layout = "${message}";

            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;

            _logger = LogManager.GetLogger("Passbook Console Log");
        }

        private static X509Certificate2 GetAppleCertificate()
        {
            X509Store certAuthStore = null;
            X509Certificate2 appleCert = null;
            X509Certificate2Collection coll = null;

            try
            {
                certAuthStore = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
                certAuthStore.Open(OpenFlags.ReadOnly);

                coll = certAuthStore.Certificates.Find(X509FindType.FindByThumbprint,
                        "ff6797793a3cd798dc5b2abef56f73edc9f83a64", false);
                if (coll.Count > 0)
                    appleCert = coll[0];

                _logger.Info(@"Apple Thumbprint: {0}", appleCert.Thumbprint);
            }
            catch (Exception ex)
            {
                _logger.Error<Exception>(ex.StackTrace, ex);
                throw;
            }

            if (appleCert == null)
            {
                throw new ArgumentNullException("appleCert", "Apple Certificate not found");
            }

            return appleCert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// SubjectKeyIdentifier DESKTOP-STORM: 96ccb973821ef44772ae65b3d022710729a97fea
        /// </remarks>
        private static X509Certificate2 GetPassCertificate()
        {
            X509Store personalStore = null;
            X509Certificate2 passCert = null;
            X509Certificate2Collection coll = null;

            try
            {
                personalStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                personalStore.Open(OpenFlags.ReadOnly);

                coll = personalStore.Certificates.Find(X509FindType.FindBySubjectKeyIdentifier,
                        "96ccb973821ef44772ae65b3d022710729a97fea", false);
                if (coll.Count > 0)
                    passCert =coll[0];

                _logger.Info(@"Pass thumbprint: {0}", passCert.Thumbprint);
            }
            catch (Exception ex)
            {
                _logger.Error<Exception>(ex.StackTrace, ex);
                throw;
            }

            if (passCert == null)
            {
                throw new ArgumentNullException("passCert", "Pass Certificate not found");
            }

            return passCert;
        }

        private static byte[] CreatePassPackage()
        {
            byte[] generatedPass = null;
            try
            {
                PassGenerator generator = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest();

                request.AppleWwdrcaCertificate = GetAppleCertificate();
                request.Certificate = GetPassCertificate();

                request.PassTypeIdentifier = "pass.datorsis.com";
                request.TeamIdentifier = "SQY9S578XQ";
                request.SerialNumber = System.Guid.NewGuid().ToString();

                request.Description = "Mega Parc";
                request.OrganizationName = "Dator Inc.";
                //request.LogoText = "Mega Parc";

                request.BackgroundColor = "rgb(255,255,255)";
                request.ForegroundColor = "rgb(0,0,0)";

                //request.Style = PassStyle.StoreCard;
                request.Style = PassStyle.EventTicket;

                var eventField = new StandardField("event", "", "eventName");
                //var cardField = new StandardField("card", "", "cardName");
                var customerField = new StandardField("customer", "customerNameLabel", "Sven Conard");
                var expirationDateField = new DateField("expirationDate", "expirationDateLabel", FieldDateTimeStyle.PKDateStyleLong, FieldDateTimeStyle.PKDateStyleNone, new DateTime(2018,12,25,12,0,0));
                var balanceField = new NumberField("balance", "balanceLabel", 43.75m, FieldNumberStyle.PKNumberStyleDecimal);
                balanceField.CurrencyCode = "CAD";
                //var balanceIntField = new NumberField("balanceInt", "balanceIntLabel", 45, FieldNumberStyle.PKNumberStyleDecimal);

                request.AddHeaderField(expirationDateField);
                //request.AddHeaderField(balanceField);
                //request.AddHeaderField(balanceIntField);
                request.AddPrimaryField(eventField);
                //request.AddPrimaryField(cardField);
                request.AddSecondaryField(customerField);
                //request.AddAuxiliaryField(expirationDateField);
                request.AddAuxiliaryField(balanceField);

                request.AddLocation(46.040017, -73.447538, "Bienvenue chez Dator!");
                //request.RelevantDate = new DateTime(2018,12,6,18,0,0,DateTimeKind.Local);
                request.RelevantDate = new DateTime(2018,12,6,18,15,0);

                //request.AddBarCode("C11127", BarcodeType.PKBarcodeFormatPDF417, "iso-8859-1", "C11127"); //"Windows-1252", "BE411604");
                request.AddBarCode("BE687136", BarcodeType.PKBarcodeFormatPDF417, "iso-8859-1", "BE687136");

                request.Images.Add(PassbookImage.Icon, File.ReadAllBytes(@"E:\Programmation\Git Repo\dotnet-passbook-sis\PassbookGeneratorConsole\icon.png"));
                request.Images.Add(PassbookImage.IconRetina, File.ReadAllBytes(@"E:\Programmation\Git Repo\dotnet-passbook-sis\PassbookGeneratorConsole\icon@2x.png"));
                request.Images.Add(PassbookImage.Logo, File.ReadAllBytes(@"E:\Programmation\Git Repo\dotnet-passbook-sis\PassbookGeneratorConsole\logo.png"));
                request.Images.Add(PassbookImage.LogoRetina, File.ReadAllBytes(@"E:\Programmation\Git Repo\dotnet-passbook-sis\PassbookGeneratorConsole\icon@2x.png"));


                request.AddLocalization("fr","eventName", "Carte Manèges");
                //request.AddLocalization("fr", "cardName", "Carte Manèges");
                request.AddLocalization("fr", "customerNameLabel", "Client");
                request.AddLocalization("fr", "expirationDateLabel", "Expire le");
                //request.AddLocalization("fr", "balanceIntLabel", "Solde");
                request.AddLocalization("fr", "balanceLabel", "Solde");
                request.AddLocalization("en","eventName", "Rides Card");
                //request.AddLocalization("en", "cardName", "Rides Card");
                request.AddLocalization("en", "customerNameLabel", "Customer");
                request.AddLocalization("en", "expirationDateLabel", "Expires on");
                //request.AddLocalization("en", "balanceIntLabel", "Balance");
                request.AddLocalization("en", "balanceLabel", "Balance");
                
                //request.AddImageLocalization("fr", "icon.png", File.ReadAllBytes(@"E:\Programmation\Git Repo\dotnet-passbook-sis\PassbookGeneratorConsole\ASPVM\icon.png"));
                //request.AddImageLocalization("fr", "logo.png", File.ReadAllBytes(@"E:\Programmation\Git Repo\dotnet-passbook-sis\PassbookGeneratorConsole\ASPVM\logo.png"));

                generatedPass = generator.Generate(request);
            }
            catch(ManifestSigningException mse)
            {
                _logger.Error<ManifestSigningException>("Manifest Signing failed.", mse);
            }
            catch(Exception ex)
            {
                _logger.Error<Exception>("Unknown error.", ex);
            }

            return generatedPass;
        }

        private static void SendPass(string passPath, bool addBcc = false)
        {
            using (var emailSender = new SmtpClient("smtp.office365.com", 587))
            {
                emailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
                emailSender.EnableSsl = true;

                emailSender.Credentials = new NetworkCredential("webadmin@datorsis.com", "202stv2684");

                using (var msg = new MailMessage("webadmin@datorsis.com", "sconard@datorsis.com", "Test Passbook", ""))
                {
                    msg.Attachments.Add(new Attachment(passPath, "application/vnd.apple.pkpass"));
                    emailSender.Send(msg);
                }
            }
        }
    }
}
