using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "ACTION", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class LemmaActionRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)-[a:ACTION]->(l:Lemma)
    /// </summary>
    public class LemmaAction : Vector<LemmaActionRelation, Regest, Lemma>
    {

    }
}
