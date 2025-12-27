using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VRC.OSCQuery
{
    public class HostInfo
    {
        [JsonInclude]
        public string name;

        [JsonInclude]
        public Dictionary<string, bool> extensions = new Dictionary<string, bool>()
        {
            { Attributes.ACCESS, true },
            { Attributes.CLIPMODE, false },
            { Attributes.RANGE, true },
            { Attributes.TYPE, true },
            { Attributes.VALUE, true },
        };
        
        [JsonInclude]
        public string oscIP;
        
        [JsonInclude]
        public int oscPort = OSCQueryService.DefaultPortOsc;

        [JsonInclude]
        public string oscTransport = Keys.OSC_TRANSPORT_UDP;

        /// <summary>
        /// Empty Constructor required for JSON Serialization
        /// </summary>
        public HostInfo()
        {
            
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public class Keys
        {
            public const string NAME = "NAME";
            public const string EXTENSIONS = "EXTENSIONS";
            public const string OSC_IP = "OSC_IP";
            public const string OSC_PORT = "OSC_PORT";
            public const string OSC_TRANSPORT = "OSC_TRANSPORT";
            public const string OSC_TRANSPORT_UDP = "UDP";
        }
    }
}