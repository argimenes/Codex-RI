using Neo4jClientVector.Core.Services;
using Services.Nodes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4jClientVector.Contexts;
using Neo4jClientVector.Helpers;
using Services.Relationships;
using Neo4jClient.Cypher;
using Neo4jClientVector.Nodes;
using Neo4jClientVector.Models;

namespace Services.Services.Places
{
    public interface IIndexPlaceService : IService<IndexPlace>
    {
        Task<IndexPlaceGraph> FindGraphAsync(Guid guid);
        Task<SearchIndexPlaceCluster> SearchAsync(SearchIndexPlaceCluster query);
    }
    public class IndexPlaceGraph : Graph<IndexPlace>
    {
        public IEnumerable<PlaceIn> Regests { get; set; }
    }
    public class IndexPlaceCluster : Cluster<IndexPlace>
    {
        public int RegestaeCount { get; set; }
        public IEnumerable<Regest> Regests { get; set; }
    }
    public class SearchIndexPlaceCluster : Search<IndexPlaceCluster>
    {
        public string Name { get; set; }
    }
    public class IndexPlaceService : Service<IndexPlace>, IIndexPlaceService
    {
        #region constructor
        public IndexPlaceService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

        public async Task<SearchIndexPlaceCluster> SearchAsync(SearchIndexPlaceCluster query)
        {
            var records = graph.From<IndexPlace>().Where()
                               .If(query.Name.HasValue(), x => x.AndWhereLike("ip.name1", query.Name))
                               .OptMatch<PlaceIn>()
                ;

            return await PageAsync<IndexPlaceCluster, SearchIndexPlaceCluster>(query, records, ip => new IndexPlaceCluster
            {
                Entity = ip.As<IndexPlace>(),
                RegestaeCount = Return.As<int>("count(distinct r)"),
                Regests = Return.As<IEnumerable<Regest>>("collect(distinct r)")
            },
            orderBy: OrderBy.From(query)
                            .When("ByName", "ip.name1")
                            .When("ByRegestaeCount", "RegestaeCount ASC, ip.name1", "RegestaeCount DESC, ip.name1 ASC")
            );
        }

        public async Task<IndexPlaceGraph> FindGraphAsync(Guid guid)
        {
            var query = graph.From<IndexPlace>(guid)
                             .OptMatch<PlaceIn>();
            return await query.FirstOrDefaultAsync(ip => new IndexPlaceGraph
            {
                Entity = ip.As<IndexPlace>(),
                Regests = Return.As<IEnumerable<PlaceIn>>(Collect<PlaceIn>())
            });
        }
    }
}
