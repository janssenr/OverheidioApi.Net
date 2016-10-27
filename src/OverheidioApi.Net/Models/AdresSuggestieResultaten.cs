using System.Runtime.Serialization;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public class AdresSuggestieResultaten
    {
        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        public AdresSuggestie[] PostcodeSuggesties { get; set; }

        [DataMember(Name = "provincienaam", EmitDefaultValue = false)]
        public AdresSuggestie[] ProvincieSuggesties { get; set; }

        [DataMember(Name = "gemeentenaam", EmitDefaultValue = false)]
        public AdresSuggestie[] GemeenteSuggesties { get; set; }

        [DataMember(Name = "woonplaatsnaam", EmitDefaultValue = false)]
        public AdresSuggestie[] WoonplaatsSuggesties { get; set; }

        [DataMember(Name = "openbareruimtenaam", EmitDefaultValue = false)]
        public AdresSuggestie[] OpenbareRuimteSuggesties { get; set; }
    }

    [DataContract]
    public class AdresSuggestie : Suggestie
    {
        [DataMember(Name = "extra", EmitDefaultValue = false)]
        public AdresExtraData ExtraData { get; set; }
    }

    [DataContract]
    public class AdresExtraData : ExtraData
    {
        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        public string Postcode { get; set; }
    }
}
