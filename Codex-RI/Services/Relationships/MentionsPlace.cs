using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "PLACE_IN", Key = "placeIn", Direction = Neo4jClient.RelationshipDirection.Incoming)]
    public class MentionsPlaceRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)&lt;-[placeIn:PLACE_IN]-(ip:IndexPlace)
    /// </summary>
    [TargetNode(Key = "place")]
    public class MentionsPlace : Vector<MentionsPlaceRelation, Regestae, IndexPlace>
    {

    }
}
