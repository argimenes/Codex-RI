using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "PERSON_IN", Key = "mentionsPerson", Direction = Neo4jClient.RelationshipDirection.Incoming)]
    public class MentionsPersonRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)&lt;-[mentionsPerson:PERSON_IN]-(person:IndexPerson)
    /// </summary>
    [TargetNode(Key = "person")]
    public class MentionsPerson : Vector<MentionsPersonRelation, Regestae, IndexPerson>
    {

    }
}
