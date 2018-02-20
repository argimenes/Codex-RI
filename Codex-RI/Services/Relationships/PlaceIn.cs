using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "PLACE_IN", Key = "placeIn", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class PlaceInRelation : Relation
    {
    }
    /// <summary>
    /// (ip:IndexPlace)-[placeIn:PLACE_IN]->(r:Regestae)
    /// </summary>
    public class PlaceIn : Vector<PlaceInRelation, IndexPlace, Regest>
    {

    }
}
