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
using System.Linq;

namespace Services.Services.Persons
{
    public class RegestGraph : Graph<Regest>
    {
        public IEnumerable<MentionsPerson> Persons { get; set; }
        public IEnumerable<MentionsPlace> PlacesMentioned { get; set; }
        public IEnumerable<PlaceOfIssue> PlacesOfIssue { get; set; }
        public Relationships.LemmaAction Action { get; set; }
    }
    public class RegestCluster : Cluster<Regest>
    {
        public IEnumerable<IndexPerson> Persons { get; set; }
        public IEnumerable<IndexPlace> PlacesMentioned { get; set; }
        public IEnumerable<Place> PlacesOfIssue { get; set; }
        public Lemma Lemma { get; set; }
    }
    public class SearchRegestCluster : Search<RegestCluster>
    {
        public string Ident { get; set; }

        public string Year { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }

        public string PlaceOfIssue { get; set; }

        public Guid? MentionedPlace { get; set; }
    }
    public interface IRegestService : IEntityService<Regest>
    {
        Task<List<string>> PlacesOfIssueAsync();
        Task<RegestGraph> FindGraphAsync(Guid guid);
        Task<List<IndexPlace>> PlacesAsync();
        Task<SearchRegestCluster> SearchAsync(SearchRegestCluster query);
    }
    public class RegestService : EntityService<Regest>, IRegestService
    {
        #region constructor
        public RegestService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

        public async Task<List<string>> PlacesOfIssueAsync()
        {
            var query = graph.From<Regest>("r").With("r");
            var result = await query.ToListAsync(r => Return.As<string>("distinct(r.placeOfIssue)"));
            return result;
        }

        public async Task<List<IndexPlace>> PlacesAsync()
        {
            var query = graph.From<IndexPlace>("p")
                             .Match<PlaceIn>(from: "p")
                             .With("p");
            var result = await query.ToListAsync(p => Return.As<IndexPlace>("distinct(p)"));
            return result;
        }

        public async Task<SearchRegestCluster> SearchAsync(SearchRegestCluster query)
        {
            var records = graph.Match<MentionsPerson>(from: "r", to: "person").Where()
                               .If(query.Year.HasValue(), x => x.AndWhere($"r.date STARTS WITH '{query.Year}'"))
                               .If(query.Month.HasValue(), x => x.AndWhereLike("r.date", query.Month))
                               .If(query.Day.HasValue(), x => x.AndWhere($"r.date ENDS WITH '{query.Day}'"))
                               .If(query.PlaceOfIssue.HasValue(), x => x.AndWhereLike("r.placeOfIssue", query.PlaceOfIssue))
                               .If(query.Ident.HasValue(), x => x.AndWhereLike("r.ident", query.Ident))
                               .If(query.MentionedPlace.HasValue, x => x.Match<MentionsPlace>(to: "place").Where("place.Guid", query.MentionedPlace.Value).With("r, person, place"))
                               .OptMatch<MentionsPlace>(to: "otherPlace")
                               .OptMatch<PlaceOfIssue>(to: "placeOfIssue")
                               .OptMatch<Relationships.LemmaAction>()
                               ;

            return await PageAsync<RegestCluster, SearchRegestCluster>(query, records, selector: r =>
            new RegestCluster
            {
                Entity = r.As<Regest>(),
                Persons = Return.As<IEnumerable<IndexPerson>>("collect(distinct(person))"),
                PlacesMentioned = Return.As<IEnumerable<IndexPlace>>("collect(distinct(otherPlace))"),
                PlacesOfIssue = Return.As<IEnumerable<Place>>("collect(distinct(placeOfIssue))"),
                Lemma = Return.As<Lemma>("head(collect(distinct l ))")
            },
            orderBy: OrderBy.From(query)
                            .When("ByDate", "r.date")
                            .When("ByPlaceOfIssue", "r.placeOfIssue")
                            .When("ByPersonCount", "PersonCount"));
        }

        public async Task<RegestGraph> FindGraphAsync(Guid guid)
        {
            var query = graph.From<Regest>("r").Where((Regest r) => r.Guid == guid)
                             .OptMatch<Relationships.LemmaAction>()
                ;
            var data = await query.FirstOrDefaultAsync(r => new
            {
                Entity = r.As<Regest>(),
                Persons = Return.As<IEnumerable<MentionsPerson>>(Rows<MentionsPerson>()),
                PlacesMentioned = Return.As<IEnumerable<MentionsPlace>>(Rows<MentionsPlace>()),
                PlacesOfIssue = Return.As<IEnumerable<PlaceOfIssue>>(Rows<PlaceOfIssue>()),
                Actions = Return.As<IEnumerable<Relationships.LemmaAction>>("collect(distinct { Relation: a, Target: l })")
            });
            return new RegestGraph
            {
                Entity = data.Entity,
                Persons = data.Persons,
                PlacesMentioned = data.PlacesMentioned,
                PlacesOfIssue = data.PlacesOfIssue,
                Action = data.Actions.FirstOrDefault()
            };
        }

        public override async Task<Result> SaveOrUpdateAsync(Regest data)
        {
            return await SaveOrUpdateAsync(data, update: x =>
            {
                x.archivalHistory = data.archivalHistory;
                x.date = data.date;
                x.ident = data.ident;
                x.placeOfIssue = data.placeOfIssue;
                x.regid = data.regid;
                x.regnum = data.regnum;
            });
        }
    }
}
