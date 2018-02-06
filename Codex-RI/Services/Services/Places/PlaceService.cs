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
    public class PlaceGraph : Graph<Place>
    {
        public IEnumerable<IssuedAt> Regestae { get; set; }
    }
    public class PlaceCluster : Cluster<Place>
    {
        public IEnumerable<Regestae> Regestae { get; set; }
        public int RegestaeCount { get; set;}
    }
    public class SearchPlaceCluster : Search<PlaceCluster>
    {
        public string Name { get; set; }
    }
    public interface IPlaceService : IEntityService<Place>
    {
        Task<PlaceGraph> FindGraphAsync(Guid guid);
        Task<List<string>> AllAsync();
        Task<SearchPlaceCluster> SearchAsync(SearchPlaceCluster query);
    }
    public class PlaceService : EntityService<Place>, IPlaceService
    {
        #region constructor
        public PlaceService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

        public async Task<List<string>> AllAsync()
        {
            var query = graph.From<Place>("p").With("p");
            var result = await query.ToListAsync(p => Return.As<string>("distinct(p.name1)"));
            return result;
        }

        public async Task<SearchPlaceCluster> SearchAsync(SearchPlaceCluster query)
        {
            var records = graph.Match<IssuedAt>().Where()
                               .If(query.Name.HasValue(), x => x.AndWhereLike("p.name", query.Name))
                               .With("p, r, count(distinct(r)) as RegestaeCount")
                               ;

            return await PageAsync<PlaceCluster, SearchPlaceCluster>(query, records, selector: r =>
            new PlaceCluster
            {
                Entity = r.As<Place>(),
                Regestae = Return.As<IEnumerable<Regestae>>("collect(distinct(r))"),
                RegestaeCount = Return.As<int>("RegestaeCount")
            },
            orderBy: OrderBy.From(query)
                            .When("ByName", "p.name")
                            .When("ByRegestaeCount", "RegestaeCount"));
        }

        public async Task<PlaceGraph> FindGraphAsync(Guid guid)
        {
            var query = graph.From<Place>()
                             .Where((Place p) => p.Guid == guid);
            return await query.FirstOrDefaultAsync(p => new PlaceGraph
            {
                Entity = p.As<Place>(),
                Regestae = Return.As<IEnumerable<IssuedAt>>(Rows<IssuedAt>()),
            });
        }

        public override async Task<Result> SaveOrUpdateAsync(Place data)
        {
            return await SaveOrUpdateAsync(data, update: x =>
            {
                x.name = data.name;
                x.lat = data.lat;
                x.@long = data.@long;
            });
        }
    }
}
