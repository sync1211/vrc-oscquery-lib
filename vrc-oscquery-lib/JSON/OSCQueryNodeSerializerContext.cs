using System.Text.Json.Serialization;
using VRC.OSCQuery;

namespace vrc_oscquery_lib
{
    [JsonSerializable(typeof(OSCQueryNode))]
    public partial class OSCQueryNodeSerializerContext: JsonSerializerContext { }
}
