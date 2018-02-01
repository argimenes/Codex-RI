using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "REFERENCES", Key = "references", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class ReferencesRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)-[references:REFERENCES]->(ref:Reference)
    /// </summary>
    [TargetNode(Key = "ref")]
    public class References : Vector<ReferencesRelation, Regestae, Reference>
    {

    }
}
