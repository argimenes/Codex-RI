using Neo4jClientVector.Attributes;
using Neo4jClientVector.Relationships;
using Services.Nodes;

namespace Services.Relationships
{
    [Relationship(Type = "DATE_OF_ISSUE", Key = "date", Direction = Neo4jClient.RelationshipDirection.Outgoing)]
    public class DateOfIssueRelation : Relation
    {
    }
    /// <summary>
    /// (r:Regestae)-[doi:DATE_OF_ISSUE]->(d:Date)
    /// </summary>
    public class DateOfIssue : Vector<DateOfIssueRelation, Regest, Date>
    {

    }
}
