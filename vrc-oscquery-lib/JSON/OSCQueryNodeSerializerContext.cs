using System.Text.Json.Serialization;
using VRC.OSCQuery;

namespace vrc_oscquery_lib
{
    [JsonSerializable(typeof(OSCQueryNode))]
    [JsonSerializable(typeof(System.String))]
    [JsonSerializable(typeof(System.Int32))]
    public partial class OSCQueryNodeSerializerContext : JsonSerializerContext { }
}
