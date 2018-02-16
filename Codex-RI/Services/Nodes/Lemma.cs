using Neo4jClientVector.Attributes;
using Neo4jClientVector.Nodes;

namespace Services.Nodes
{
    [Ident(Property = "lemma")]
    public class Lemma : Entity
    {
        public string lemma { get; set; }
        public new string DisplayName
        {
            get
            {
                return lemma;
            }
        }
    }
}
