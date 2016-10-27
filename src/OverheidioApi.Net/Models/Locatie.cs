using System.Runtime.Serialization;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public class Locatie
    {
        [DataMember(Name = "lat", EmitDefaultValue = false)]
        public string Lat { get; set; }

        [DataMember(Name = "lon", EmitDefaultValue = false)]
        public string Lon { get; set; }
    }
}
