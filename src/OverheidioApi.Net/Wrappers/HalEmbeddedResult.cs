using System.Collections.Generic;
using System.Runtime.Serialization;
using OverheidioApi.Net.Models;

namespace OverheidioApi.Net.Wrappers
{
    [DataContract]
    public sealed class HalEmbeddedResult
    {
        [DataMember(Name = "rechtspersoon", EmitDefaultValue = false)]
        public List<Bedrijf> Bedrijven { get; set; }

        [DataMember(Name = "kenteken", EmitDefaultValue = false)]
        public List<Voertuig> Voertuigen { get; set; }

        [DataMember(Name = "adres", EmitDefaultValue = false)]
        public List<Adres> Adressen { get; set; }
    }
}
