using Neo4jClientVector.Attributes;
using Neo4jClientVector.Nodes;

namespace Services.Nodes
{
    public class Regestae : Entity
    {
        public string archivalHistory { get; set; }
        public string date { get; set; }
        public string ident { get; set; }
        public string placeOfIssue { get; set; }
        public string regid { get; set; }
        public string regnum { get; set; }
        public string text { get; set; }
    }
}
