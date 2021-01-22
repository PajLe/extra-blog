using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ExtraBlog.DTOs;
using Neo4jClient;
using ExtraBlog.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ExtraBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGraphClient _context;

        public UserController(IGraphClient graphClient)
        {
            _context = graphClient;
        }

        [HttpGet("newsfollow/{username}")]
        public async Task<IActionResult> GetNewsFollowing(string username)
        {
            var users = await _context.Cypher.Match("(f:User)<-[:FOLLOW]-(n:User), (d:Document{isArchived: false})")
                                             .Where($"NOT(n.isArchived AND f.isArchived) AND n.Username=~'{username}' AND f.Username=~ d.CreatedBy")
                                             .Return<Document>("d")
                                             .OrderBy("d.Created")
                                             .Limit(10)
                                             .ResultsAsync;

            return new JsonResult(users);
        }

        [HttpGet("news/{username}")]
        public async Task<IActionResult> GetNewsFeed(string username)
        {
            return new JsonResult(await _context.Cypher.Match($"(a:User{{Username: '{username}'}})-[:INTERESTED_IN]->(res:Category)<-[:TAG]-(b:Document)")
                                                       //.Where($"NOT(b.isArchived AND c.isArchived AND b.CreatedBy =~ '{username}')")
                                                       .With("b.name AS name, b.CreatedBy AS creator, b.Pictures AS pictures, b.Paragraphs AS paragraphs, COUNT(res) AS interest1")
                                                       //.Return<Document>("b, interest1")
                                                       .Return((name, creator, pictures, paragraphs, interest1)=> new SimpleNewsFeedDTO { 
                                                                                                                                           Name = name.As<string>()
                                                                                                                                           , Creator = creator.As<string>()
                                                                                                                                           , Pictures = pictures.As<string[]>()
                                                                                                                                           , Paragraphs = paragraphs.As<string[]>()
                                                                                                                                           , Interest = interest1.As<int>() })  
                                                       .OrderBy("interest1 DESC")
                                                       .Limit(10)
                                                       .ResultsAsync);
        }

        [HttpGet("following/{username}")]
        public async Task<IActionResult> GetFollows(string username)
        {
            return new JsonResult(await _context.Cypher.Match($"(n:User{{Username: '{username}'}})-[:FOLLOW]->(f:User)")
                                                      .Return<string>("f.Username").ResultsAsync);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            //var result = await _context.Cypher.Match("(c:Category)<--(n:User)-->(f:User)")
            //.Where($"NOT(n.isArchived AND c.isArchived AND f.isArchived) AND n.Username=~'{username}' AND ")
            //.Return<UserDTO>("n.Username AS Username, collect(f.Username) AS Users, collect(c.name) AS Categories")
            //.With("n.Username AS Username, collect(f.Username) AS Users, collect(c.name) AS Categories")
            //.Return((Username, Users, Categories) => new UserDTO { Username = Username.As<string>(), Users = Users.As<Collection<string>>(), Categories = Categories.As<Collection<string>>()})
            //.ResultsAsync;
            var users = await _context.Cypher.Match("(f:User)<-[:FOLLOW]-(n:User)")
                                             .Where($"NOT(n.isArchived AND f.isArchived) AND n.Username=~'{username}'")
                                             .Return<string>("f.Username").ResultsAsync;
            var categories = await _context.Cypher.Match("(c:Category)<-[:INTERESTED_IN]-(n:User)")
                                             .Where($"NOT(n.isArchived AND c.isArchived) AND n.Username=~'{username}'")
                                             .Return<string>("c.name").ResultsAsync;
            //if (!result.Any()) { return BadRequest("Document doesn't exist"); }
            
            UserDTO result = new UserDTO();
            result.Username = username;
            result.Categories = categories.ToList();
            result.Users = users.ToList();
            return new JsonResult(result);
        }

        [HttpPut("follow/{username}")]
        public async Task AddFollowRealtionship(string username, [FromBody] string friend)
        {
            
            await _context.Cypher.Match("(u:User), (f:User)")
                                 .Where($"NOT(u.isArchived AND f.isArchived) AND u.Username =~ '{username}' AND f.Username=~ '{friend}'")
                                 .Merge("(u)-[r: FOLLOW]->(f)")
                                 .ExecuteWithoutResultsAsync();
        }
    }
}