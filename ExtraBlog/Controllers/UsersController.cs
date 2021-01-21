using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ExtraBlog.DTOs;
using Neo4jClient;
using ExtraBlog.Models;

namespace ExtraBlog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGraphClient _context;

        public UserController(IGraphClient graphClient)
        {
            _context = graphClient;
        }

        [HttpGet("news/{username}")]
        public async Task<IActionResult> GetNewsFeed(string username)
        {
            return new JsonResult(await _context.Cypher.Match($"(a:User{{Username: '{username}'}})-[:INTERESTED_IN]->(res:Category)<-[:TAG]-(b:Document)")
                                                       .OptionalMatch($"(a:User{{Username: '{username}'}})-[f:FOLLOW]->(u:User), (b:Document{{CreatedBy: u.Username}})")
                                                       .With("b, COUNT(res) AS interest1, COUNT(u) AS interest2")
                                                       .Return<Document>("b, interest1 + interest2 AS interest")
                                                       .OrderBy("interest DESC")
                                                       .Limit(10)
                                                       .ResultsAsync);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollows(string username)
        {
            return new JsonResult(await _context.Cypher.Match($"(n:User{{Username: '{username}'}})-[:FOLLOW]->(f:User)")
                                                      .Return<string>("f.Username").ResultsAsync);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var result = await _context.Cypher.Match("(c:Category)<--(n:User)-->(f:User)")
                                              .Where($"NOT(n.isArchived AND c.isArchived AND f.isArchived) AND n.Username=~'{username}' AND ")
                                              .Return<UserDTO>("n.Username AS Username, collect(f.Username) AS Users, collect(c.name) AS Categories").ResultsAsync;
            if (!result.Any()) { return BadRequest("Document doesn't exist"); }

            return new JsonResult(result);
        }

        [HttpPut("follow/{documentname}")]
        public async Task AddFollowRealtionship(string username, [FromBody] string friend)
        {
            
            await _context.Cypher.Match("(u:User), (f:Category)")
                                 .Where($"NOT(u.isArchived AND f.isArchived) AND u.Username =~ '{username}' AND f.Username=~ '{friend}'")
                                 .Merge("(u)-[r: FOLLOW]->(f)")
                                 .ExecuteWithoutResultsAsync();
        }
    }
}