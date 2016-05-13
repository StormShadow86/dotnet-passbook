using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Passbook.Generator.Fields;

namespace Passbook.Generator
{
    public class JsonTemplateProvider
    {
        private JsonTemplateHolder _holder;
        
        public void LoadJsonTemplateHolder()
        {
            throw new NotImplementedException();
        }

        public void SaveJsonTemplate(JsonTemplate template)
        {
            throw new NotImplementedException();
        }

        public JsonTemplate LoadJsonTemplate(string templateName)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonTemplateHolder
    {
        public AppleWWDRCACertificateInfo AppleWWDRCACertificate { get; set; }
        public List<JsonTemplate> JsonTemplates { get; set; }
    }

    public class AppleWWDRCACertificateInfo
    {
        public string AppleWWDRCACertificatePath { get; set; }
        public string AppleWWDRCACertificateThumbprint { get; set; }
        public StoreLocation AppleWWDRCACertificateStoreLocation { get; set; }
    }

    public class CertificateInfo
    {
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }
        public string CertificateThumbprint { get; set; }
        public string CertificateSubjectKeyIdentifier { get; set; }
        public StoreLocation CertificateStoreLocation { get; set; }
    }

    public class JsonTemplate
    {
        public string Name { get; set; }

        public string PassTypeIdentifier { get; set; }
        public string TeamIdentifier { get; set; }
        public PassStyle Style { get; set; }
        public CertificateInfo Certificate { get; set; }
        public string Description { get; set; }
        public string OrganizationName { get; set; }       

        public string AppLaunchURL { get; set; }
        public List<int> AssociatedStoreIdentifiers { get; set; }

        public List<RelevantBeacon> Beacons { get; set; }
        public List<RelevantLocation> Locations { get; set; }
        public int? MaxDistance { get; set; }

        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string LabelColor { get; set; }
        public string GroupingIdentifier { get; set; }
        public string LogoText { get; set; }
        public bool? SuppressStripShine { get; set; }

        public string AuthenticationToken { get; set; }
        public string WebServiceURL { get; set; }



        public List<Field> HeaderFields { get; set; }
        public List<Field> PrimaryFields { get; set; }
        public List<Field> SecondaryFields { get; set; }
        public List<Field> AuxiliaryFields { get; set; }
        public List<Field> BackFields { get; set; }

        public TransitType TransitType { get; set; }

        public Dictionary<PassbookImage, string> Images { get; set; }
        public Dictionary<string, Dictionary<string, string>> Localizations { get; set; }
    }    
}
