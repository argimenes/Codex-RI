using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "PLACE_OF_ISSUE", Direction = Neo4jClient.RelationshipDirection.Incoming)]
    public class IssuedAtRelation : Relation
    {
    }
    /// <summary>
    /// (p:Place)&lt;-[iop:PLACE_OF_ISSUE]-(r:Regestae)
    /// </summary>
    public class IssuedAt : Vector<IssuedAtRelation, Place, Regestae>
    {

    }
}
