using Neo4jClientVector.Attributes;
using Neo4jClientVector.Nodes;

namespace Services.Nodes
{
    [Ident(Property = "url")]
    public class Reference : Entity
    {
        public string title { get; set; }
        public string url { get; set; }
    }
}
