using System.Runtime.Serialization;

namespace OverheidioApi.Net.Models
{
    [DataContract]
    public abstract class Suggestie
    {
        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }
    }

    [DataContract]
    public abstract class ExtraData
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }
    }
}
