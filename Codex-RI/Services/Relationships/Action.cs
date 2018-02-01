using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "ACTION", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class ActionRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)-[a:ACTION]->(l:Lemma)
    /// </summary>
    public class Action : Vector<ActionRelation, Regestae, Lemma>
    {

    }
}
