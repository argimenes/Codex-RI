using Neo4jClientVector.Attributes;
using Neo4jClientVector.Nodes;

namespace Services.Nodes
{
    [Ident(Property = "registerid")]
    public class IndexPlace : Entity
    {
        public string registerid { get; set; }
        public string name1 { get; set; }
    }
}
