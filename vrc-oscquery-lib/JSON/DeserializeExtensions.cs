using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VRC.OSCQuery;

namespace vrc_oscquery_lib.JSON
{
    public static class DeserializeExtensions
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            Converters = { new BooleanConverter() }
        };

        public static TValue Deserialize<TValue>(this JsonElement element)
        {
            return JsonSerializer.Deserialize<TValue>(element, options);
        }
    }
}
