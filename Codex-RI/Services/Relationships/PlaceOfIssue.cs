using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "PLACE_OF_ISSUE", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class PlaceOfIssueRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)-[poi:PLACE_OF_ISSUE]->(p:Place)
    /// </summary>
    public class PlaceOfIssue : Vector<PlaceOfIssueRelation, Regest, Place>
    {

    }
}
