using Neo4jClientVector.Services;
using Services.Nodes;
using System.Threading.Tasks;
using Neo4jClientVector.Contexts;
using Neo4jClientVector.Models;
using Neo4jClientVector.Nodes;
using Services.Relationships;
using System.Collections.Generic;
using Neo4jClientVector.Helpers;
using Neo4jClient.Cypher;
using System;

namespace Services.Services.Persons
{
    public class PersonGraph : Graph<IndexPerson>
    {
        public IEnumerable<PersonIn> Regests { get; set; }
    }
    public class PersonCluster : Cluster<IndexPerson>
    {
        public IEnumerable<Regestae> Regests { get; set; }
    }
    public class SearchPersonCluster : Search<PersonCluster>
    {
        public string RegisterId { get; set; }
        public string Name1 { get; set; }
        public string Name3 { get; set; }
    }
    public interface IPersonService : IEntityService<IndexPerson>
    {
        Task<PersonGraph> FindGraphAsync(Guid guid);
        Task<SearchPersonCluster> SearchAsync(SearchPersonCluster query);
    }
    public class PersonService : EntityService<IndexPerson>, IPersonService
    {
        #region constructor
        public PersonService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

        public async Task<SearchPersonCluster> SearchAsync(SearchPersonCluster query)
        {
            var records = graph.Match<PersonIn>(from: "p").Where()
                               .If(query.Name1.HasValue(), x => x.AndWhereLike("p.name1", query.Name1))
                               .If(query.Name3.HasValue(), x => x.AndWhereLike("p.name3", query.Name3))
                               ;

            return await PageAsync<PersonCluster, SearchPersonCluster>(query, records, selector: p =>
            new PersonCluster
            {
                Entity = p.As<IndexPerson>(),
                Regests = Return.As<IEnumerable<Regestae>>("collect(distinct(r))")
            },
            orderBy: OrderBy.From(query).When("ByName", "p.name1"));
        }

        public async Task<PersonGraph> FindGraphAsync(Guid guid)
        {
            var query = graph.From<IndexPerson>("person")
                             .Where((IndexPerson person) => person.Guid == guid)
                             .Match<PersonIn>();
            return await query.FirstOrDefaultAsync(person => new PersonGraph
            {
                Entity = person.As<IndexPerson>(),
                Regests = Return.As<IEnumerable<PersonIn>>(Collect<PersonIn>())
            });
        }

        public override async Task<Result> SaveOrUpdateAsync(IndexPerson data)
        {
            return await SaveOrUpdateAsync(data, update: x =>
            {
                x.name1 = data.name1;
                x.name3 = data.name3;
                x.registerid = data.registerid;
            });
        }
    }
}
