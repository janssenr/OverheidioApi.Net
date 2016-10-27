using System.Runtime.Serialization;
using OverheidioApi.Net.Wrappers;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public class Adres
    {
        [DataMember(Name = "gemeentenaam", EmitDefaultValue = false)]
        public string Gemeentenaam { get; set; }

        [DataMember(Name = "huisletter", EmitDefaultValue = false)]
        public string Huisletter { get; set; }

        [DataMember(Name = "huisnummer", EmitDefaultValue = false)]
        public string Huisnummer { get; set; }

        [DataMember(Name = "huisnummertoevoeging", EmitDefaultValue = false)]
        public string HuisnummerToevoeging { get; set; }

        [DataMember(Name = "locatie", EmitDefaultValue = false)]
        public Locatie Locatie { get; set; }

        [DataMember(Name = "openbareruimtenaam", EmitDefaultValue = false)]
        public string Openbareruimtenaam { get; set; }

        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        public string Postcode { get; set; }

        [DataMember(Name = "provincienaam", EmitDefaultValue = false)]
        public string Provincienaam { get; set; }

        [DataMember(Name = "typeadresseerbaarobject", EmitDefaultValue = false)]
        public string TypeAdresseerbaarObject { get; set; }

        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }

        [DataMember(Name = "woonplaatsnaam", EmitDefaultValue = false)]
        public string Woonplaatsnaam { get; set; }

        [DataMember(Name = "_links", EmitDefaultValue = false)]
        public HalNavigator Links { get; set; }
    }
}
