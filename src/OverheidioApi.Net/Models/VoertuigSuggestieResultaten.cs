using System.Runtime.Serialization;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public class VoertuigSuggestieResultaten
    {
        [DataMember(Name = "eerstekleur", EmitDefaultValue = false)]
        public VoertuigSuggestie[] EersteKleurSuggesties { get; set; }

        [DataMember(Name = "handelsbenaming", EmitDefaultValue = false)]
        public VoertuigSuggestie[] HandelsbenamingSuggesties { get; set; }

        [DataMember(Name = "hoofdbrandstof", EmitDefaultValue = false)]
        public VoertuigSuggestie[] HoofdbrandstofSuggesties { get; set; }

        [DataMember(Name = "inrichting", EmitDefaultValue = false)]
        public VoertuigSuggestie[] InrichtingSuggesties { get; set; }

        [DataMember(Name = "merk", EmitDefaultValue = false)]
        public VoertuigSuggestie[] MerkSuggesties { get; set; }

        [DataMember(Name = "milieuclassificatie", EmitDefaultValue = false)]
        public VoertuigSuggestie[] MilieuclassificatieSuggesties { get; set; }

        [DataMember(Name = "nevenbrandstof", EmitDefaultValue = false)]
        public VoertuigSuggestie[] NevenbrandstofSuggesties { get; set; }

        [DataMember(Name = "voertuigsoort", EmitDefaultValue = false)]
        public VoertuigSuggestie[] VoertuigsoortSuggesties { get; set; }
    }

    [DataContract]
    public class VoertuigSuggestie : Suggestie
    {
        [DataMember(Name = "extra", EmitDefaultValue = false)]
        public VoertuigExtraData ExtraData { get; set; }
    }

    [DataContract]
    public class VoertuigExtraData : ExtraData
    {
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }
    }
}
