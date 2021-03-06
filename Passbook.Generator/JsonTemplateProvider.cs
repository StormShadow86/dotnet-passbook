﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passbook.Generator.Fields;

namespace Passbook.Generator
{
    public class JsonTemplateProvider
    {
        private JsonTemplateHolder _holder;
        private string _filePath;
        
        public void LoadJsonTemplateHolder(string filePath)
        {
            _filePath = filePath;
            _holder = new JsonTemplateHolder();

            using (FileStream fs = new FileStream(_filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                            }
                            else
                            {
                               Console.WriteLine("Token: {0}", reader.TokenType); 
                            }
                        }
                    }
                }
                
            }           
        }

        public void SaveJsonTemplate(JsonTemplate template)
        {
            if(_holder != null)
            {
                if (!_holder.JsonTemplates.Contains(template))
                {
                    _holder.JsonTemplates.Add(template);
                }
                else
                {
                    int pos = _holder.JsonTemplates.IndexOf(template);
                    if (pos >= 0 && pos < _holder.JsonTemplates.Count)
                    {
                        _holder.JsonTemplates[pos] = template;
                    }
                }
                SaveJsonTemplateHolder(_filePath);
            }            
        }

        public void SaveJsonTemplateHolder(string filepath)
        {
            throw new NotImplementedException();
        }

        public JsonTemplate LoadJsonTemplate(string templateName)
        {
            JsonTemplate result;
            if (_holder == null)
                throw new InvalidOperationException("JsonTemplateHolder is not loaded.");

            result = _holder.JsonTemplates.Find(x => x.Name == templateName);

            if (result == null)
            {
                throw new KeyNotFoundException("Template not found");
            }

            return result;
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class JsonTemplateHolder
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include 
            ,PropertyName = "appleWWDRCACertificate"
            ,Required = Required.Always
            ,Order = 1)]
        public AppleWwdrcaCertificateInfo AppleWwdrcaCertificate { get; set; }
        

        public List<JsonTemplate> JsonTemplates { get; set; }
    }

    public class AppleWwdrcaCertificateInfo
    {
        public string AppleWwdrcaCertificatePath { get; set; }
        public string AppleWwdrcaCertificateThumbprint { get; set; }
        public string AppleWwdrcaCertificateSubjectKeyIdentifier { get; set; }
        public string AppleWwdrcaCertificateSerialNumber { get; set; }
        public StoreLocation AppleWwdrcaCertificateStoreLocation { get; set; }
        public StoreName AppleWwdrcaCertificateStoreName { get; set; }
        public X509FindType AppleWwdrcaFindType { get; set; }
    }

    public class CertificateInfo
    {
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }
        public string CertificateThumbprint { get; set; }
        public string CertificateSubjectKeyIdentifier { get; set; }
        public string CertificateSerialNumber { get; set; }
        public StoreLocation CertificateStoreLocation { get; set; }
        public StoreName CertificateStoreName { get; set; }
        public X509FindType CertificateFindType { get; set; }
    }

    [JsonArray(AllowNullItems = false
            , Id = "templates")]
    public class JsonTemplate
    {
        public string Name { get; set; }

        public string PassTypeIdentifier { get; set; }
        public string TeamIdentifier { get; set; }
        public PassStyle Style { get; set; }
        public CertificateInfo Certificate { get; set; }
        public string Description { get; set; }
        public string OrganizationName { get; set; }       

        public string AppLaunchUrl { get; set; }
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
        public string WebServiceUrl { get; set; }

        public List<NfcPayload> NfcPayloads { get; set; }

        public List<Field> HeaderFields { get; set; }
        public List<Field> PrimaryFields { get; set; }
        public List<Field> SecondaryFields { get; set; }
        public List<Field> AuxiliaryFields { get; set; }
        public List<Field> BackFields { get; set; }

        public TransitType TransitType { get; set; }

        public Dictionary<PassbookImage, string> Images { get; set; }
        public Dictionary<string, Dictionary<string, string>> Localizations { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            JsonTemplate other = obj as JsonTemplate;
            if(other == null)
            {
                return false;
            }
            else
            {
                return this.Name.Equals(other.Name,StringComparison.InvariantCulture);
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }    
}
