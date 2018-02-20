using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "PERSON_IN", Key = "personIn", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class PersonInRelation : Relation
    {
    }
    /// <summary>
    /// (person:IndexPerson)-[personIn:PERSON_IN]->(r:Regestae)
    /// </summary>
    [TargetNode(Key = "person")]
    public class PersonIn : Vector<PersonInRelation, IndexPerson, Regest>
    {

    }
}
