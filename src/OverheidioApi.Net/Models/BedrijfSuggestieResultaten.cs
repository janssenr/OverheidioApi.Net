using System.Runtime.Serialization;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public class BedrijfSuggestieResultaten
    {
        [DataMember(Name = "handelsnaam", EmitDefaultValue = false)]
        public BedrijfSuggestie[] HandelsnaamSuggesties { get; set; }

        [DataMember(Name = "straat", EmitDefaultValue = false)]
        public BedrijfSuggestie[] StraatSuggesties { get; set; }

        [DataMember(Name = "dossiernummer", EmitDefaultValue = false)]
        public BedrijfSuggestie[] DossiernummerSuggesties { get; set; }
    }

    [DataContract]
    public class BedrijfSuggestie : Suggestie
    {
        [DataMember(Name = "extra", EmitDefaultValue = false)]
        public BedrijfExtraData ExtraData { get; set; }
    }

    [DataContract]
    public class BedrijfExtraData : ExtraData
    {
        [DataMember(Name = "handelsnaam", EmitDefaultValue = false)]
        public string Handelsnaam { get; set; }

        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        public string Postcode { get; set; }
    }
}
