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
    }
    public class IndexPlaceGraph : Graph<IndexPlace>
    {
        public IEnumerable<PlaceIn> Regests { get; set; }
    }
    public class IndexPlaceCluster : Cluster<IndexPlace>
    {
        public Regestae Regesta { get; set; }
    }
    public class IndexPlaceService : Service<IndexPlace>, IIndexPlaceService
    {
        #region constructor
        public IndexPlaceService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

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
