using System.Text.Json.Serialization;
using VRC.OSCQuery;

namespace vrc_oscquery_lib
{
    [JsonSerializable(typeof(HostInfo))]
    public partial class HostInfoSerializerContext : JsonSerializerContext { }
}
