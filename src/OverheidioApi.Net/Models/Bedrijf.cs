using System.Runtime.Serialization;
using OverheidioApi.Net.Wrappers;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public class Bedrijf
    {
        [DataMember(Name = "actief", EmitDefaultValue = false)]
        public bool Actief { get; set; }

        [DataMember(Name = "bestaandehandelsnaam", EmitDefaultValue = false)]
        public string BestaandeHandelsnaam { get; set; }

        [DataMember(Name = "dossiernummer", EmitDefaultValue = false)]
        public string Dossiernummer { get; set; }

        [DataMember(Name = "handelsnaam", EmitDefaultValue = false)]
        public string Handelsnaam { get; set; }

        [DataMember(Name = "handelsnaam_url", EmitDefaultValue = false)]
        public string HandelsnaamUrl { get; set; }

        [DataMember(Name = "huisnummer", EmitDefaultValue = false)]
        public string Huisnummer { get; set; }

        [DataMember(Name = "huisnummertoevoeging", EmitDefaultValue = false)]
        public string Huisnummertoevoeging { get; set; }

        [DataMember(Name = "plaats", EmitDefaultValue = false)]
        public string Plaats { get; set; }

        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        public string Postcode { get; set; }

        [DataMember(Name = "straat", EmitDefaultValue = false)]
        public string Straat { get; set; }

        [DataMember(Name = "straat_url", EmitDefaultValue = false)]
        public string StraatUrl { get; set; }

        [DataMember(Name = "subdossiernummer", EmitDefaultValue = false)]
        public string Subdossiernummer { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(Name = "vestigingsnummer", EmitDefaultValue = false)]
        public int Vestigingsnummer { get; set; }

        [DataMember(Name = "_links", EmitDefaultValue = false)]
        public HalNavigator Links { get; set; }
    }
}
