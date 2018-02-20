using Neo4jClientVector.Core.Services;
using Services.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClientVector.Contexts;
using Neo4jClientVector.Nodes;
using Neo4jClientVector.Helpers;
using Services.Relationships;
using Neo4jClient.Cypher;
using Neo4jClientVector.Models;

namespace Services.Services.Regestae
{
    public interface ILemmaService : IService<Lemma>
    {
        Task<LemmaGraph> FindGraphAsync(Guid id);
        Task<SearchLemmaCluster> SearchAsync(SearchLemmaCluster search);
    }
    public class LemmaGraph : Graph<Lemma>
    {
        public IEnumerable<LemmaAction> Actions { get; set; }
    }
    public class LemmaCluster : Cluster<Lemma>
    {
        public int RegestCount { get; set; }
    }
    public class SearchLemmaCluster : Search<LemmaCluster>
    {
        public string Lemma { get; set; }
    }
    public class LemmaService : Service<Lemma>, ILemmaService
    {
        #region constructor
        public LemmaService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

        public async Task<LemmaGraph> FindGraphAsync(Guid guid)
        {
            var query = graph.From<Lemma>(guid);

            return await query.FirstOrDefaultAsync(l => new LemmaGraph
            {
                Entity = l.As<Lemma>(),
                Actions = Return.As<IEnumerable<LemmaAction>>(Rows<LemmaAction>())
            });
        }

        public async Task<SearchLemmaCluster> SearchAsync(SearchLemmaCluster search)
        {
            var records = graph.From<Lemma>().Where()
                               .If(search.Lemma.HasValue(), x => x.AndWhereLike("l.lemma", search.Lemma))
                               .OptMatch<LemmaAction>()
                              ;

            return await PageAsync<LemmaCluster, SearchLemmaCluster>(search, records, l => new LemmaCluster
            {
                Entity = l.As<Lemma>(),
                RegestCount = Return.As<int>("count(distinct r)")
            }, orderBy: OrderBy.From(search)
                               .When("ByName", "l.lemma")
                               .When("ByRegestCount", "RegestCount ASC, l.lemma ASC", "RegestCount DESC, l.lemma ASC"));
        }
    }
}
