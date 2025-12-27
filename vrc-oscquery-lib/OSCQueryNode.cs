using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace VRC.OSCQuery
{
    
    public class OSCQueryRootNode : OSCQueryNode
    {
        private Dictionary<string, OSCQueryNode> _pathLookup;

        public OSCQueryRootNode()
        {
            // Initialize path lookup with self reference
            _pathLookup = new Dictionary<string, OSCQueryNode>()
            {
                {"/", this}
            };
            
        }
        public OSCQueryNode GetNodeWithPath(string path)
        {
            if (_pathLookup == null)
            {
                RebuildLookup();
            }
            
            if (_pathLookup.TryGetValue(path, out OSCQueryNode node))
            {
                return node;
            }
            
            return null;
        }
        
        public OSCQueryNode AddNode(OSCQueryNode node)
        {
            // Todo: parse path and figure out which sub-node to add it to
            var parent = GetNodeWithPath(node.ParentPath);
            if (parent == null)
            {
                parent = AddNode(new OSCQueryNode(node.ParentPath));
            }
            if (parent.Contents == null)
            {
                parent.Contents = new Dictionary<string, OSCQueryNode>();
            }
            else if (parent.Contents.ContainsKey(node.Name))
            {
                OSCQueryService.Logger.LogWarning($"Child node {node.Name} already exists on {FullPath}, you need to remove the existing entry first");
                return null;
            }

            // Add to contents
            parent.Contents.Add(node.Name, node);
            
            // Todo: handle case where this full path already exists, but I don't think it should ever happen
            _pathLookup.Add(node.FullPath, node);
            
            return node;
        }

        public bool RemoveNode(string path)
        {
            if(_pathLookup.TryGetValue(path, out OSCQueryNode node))
            {
                var parent = GetNodeWithPath(node.ParentPath);
                if (parent?.Contents != null)
                {
                    if (parent.Contents.ContainsKey(node.Name))
                    {
                        parent.Contents.Remove(node.Name);
                        _pathLookup.Remove(path);
                        return true;
                    }
                }
            }
            return false;
        }

        public void RebuildLookup()
        {
            _pathLookup = new Dictionary<string, OSCQueryNode>()
            {
                { "/", this },
            };
            AddContents(this);
        }

        /// <summary>
        /// Recursive Function to rebuild Lookup
        /// </summary>
        /// <param name="node"></param>
        public void AddContents(OSCQueryNode node)
        {
            // Don't try to add null contents
            if (node.Contents == null)
            {
                return;
            }

            foreach (var subNode in node.Contents.Select(pair => pair.Value))
            {
                _pathLookup.Add(subNode.FullPath, subNode);
                if (subNode.Contents != null)
                {
                    AddContents(subNode);
                }
            }
        }

        public static OSCQueryRootNode FromString(string json)
        {
            var tree = JsonSerializer.Deserialize<OSCQueryRootNode>(json);
            tree.RebuildLookup();
            return tree;
        }
    }
    public class OSCQueryNode
    {
        // Empty Constructor for Json Serialization
        public OSCQueryNode(){}

        public OSCQueryNode(string fullPath)
        {
            FullPath = fullPath;
        }
        
        [JsonInclude]
        [JsonPropertyName(Attributes.DESCRIPTION)]
        public string Description;

        [JsonInclude]
        [JsonPropertyName(Attributes.FULL_PATH)]
        public string FullPath;

        [JsonInclude]
        [JsonPropertyName(Attributes.ACCESS)]
        public Attributes.AccessValues Access;

        [JsonInclude]
        [JsonPropertyName(Attributes.CONTENTS)]
        public Dictionary<string, OSCQueryNode> Contents;

        [JsonInclude]
        [JsonPropertyName(Attributes.TYPE)]
        public string OscType;

        [JsonInclude]
        [JsonPropertyName(Attributes.VALUE)]
        public object[] Value;

        [JsonIgnore]
        public string ParentPath
        {
            get
            {
                int length = Math.Max(1, FullPath.LastIndexOf("/"));
                return FullPath.Substring(0, length);
            }
            
        }

        [JsonIgnore]
        public string Name => FullPath.Substring(FullPath.LastIndexOf('/')+1);

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}