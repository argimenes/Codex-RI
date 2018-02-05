using Neo4jClientVector.Attributes;
using Neo4jClientVector.Nodes;

namespace Services.Nodes
{
    public class Place : Entity
    {
        public float? lat { get; set; }
        public float? @long { get; set; }
        public string name { get; set; }
    }
}
