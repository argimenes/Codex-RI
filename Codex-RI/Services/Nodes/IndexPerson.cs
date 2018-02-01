using Neo4jClientVector.Attributes;
using Neo4jClientVector.Nodes;

namespace Services.Nodes
{
    [Node(Key = "person")]
    [Ident(Property = "registerid")]
    public class IndexPerson : Entity
    {
        public string registerid { get; set; }
        public string name1 { get; set; }
        public string name3 { get; set; }
    }
}
