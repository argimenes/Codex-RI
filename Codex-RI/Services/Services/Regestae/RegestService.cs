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
    public class RegestGraph : Graph<Regestae>
    {
        public IEnumerable<MentionsPerson> Persons { get; set; }
        public IEnumerable<MentionsPlace> PlacesMentioned { get; set; }
        public IEnumerable<PlaceOfIssue> PlacesOfIssue { get; set; }
    }
    public class RegestCluster : Cluster<Regestae>
    {
        public IEnumerable<IndexPerson> Persons { get; set; }
        public IEnumerable<IndexPlace> PlacesMentioned { get; set; }
        public IEnumerable<Place> PlacesOfIssue { get; set; }
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
    public interface IRegestService : IEntityService<Regestae>
    {
        Task<List<string>> PlacesOfIssueAsync();
        Task<RegestGraph> FindGraphAsync(Guid guid);
        Task<List<IndexPlace>> PlacesAsync();
        Task<SearchRegestCluster> SearchAsync(SearchRegestCluster query);
    }
    public class RegestService : EntityService<Regestae>, IRegestService
    {
        #region constructor
        public RegestService(IGraphDataContext _db) : base(_db)
        {
        }
        #endregion

        public async Task<List<string>> PlacesOfIssueAsync()
        {
            var query = graph.From<Regestae>("r").With("r");
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
                               ;

            return await PageAsync<RegestCluster, SearchRegestCluster>(query, records, selector: r =>
            new RegestCluster
            {
                Entity = r.As<Regestae>(),
                Persons = Return.As<IEnumerable<IndexPerson>>("collect(distinct(person))"),
                PlacesMentioned = Return.As<IEnumerable<IndexPlace>>("collect(distinct(otherPlace))"),
                PlacesOfIssue = Return.As<IEnumerable<Place>>("collect(distinct(placeOfIssue))"),
            },
            orderBy: OrderBy.From(query)
                            .When("ByDate", "r.date")
                            .When("ByPlaceOfIssue", "r.placeOfIssue")
                            .When("ByPersonCount", "PersonCount"));
        }

        public async Task<RegestGraph> FindGraphAsync(Guid guid)
        {
            var query = graph.From<Regestae>("r")
                             .Where((Regestae r) => r.Guid == guid);
            return await query.FirstOrDefaultAsync(r => new RegestGraph
            {
                Entity = r.As<Regestae>(),
                Persons = Return.As<IEnumerable<MentionsPerson>>(Rows<MentionsPerson>()),
                PlacesMentioned = Return.As<IEnumerable<MentionsPlace>>(Rows<MentionsPlace>()),
                PlacesOfIssue = Return.As<IEnumerable<PlaceOfIssue>>(Rows<PlaceOfIssue>())
            });
        }

        public override async Task<Result> SaveOrUpdateAsync(Regestae data)
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
