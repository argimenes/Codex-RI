using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "OBERBEGRIFF", Key = "parentOf", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class ParentOfRelation : Relation
    {
    }
    /// <summary>
    /// (parent:RegisterOrt)-[parentOf:OBERBEGRIFF]->(child:RegisterOrt)
    /// </summary>
    [SourceVector(Key = "parent"), TargetVector(Key = "child")]
    public class ParentOf : Vector<ParentOfRelation, IndexPerson, IndexPerson>
    {

    }
}
