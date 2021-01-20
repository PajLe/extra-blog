using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtraBlog.DTOs;
using ExtraBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExtraBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGraphClient _context;

        public CategoryController(IGraphClient graphClient)
        {
            /*
            _context = new GraphClient(new Uri("http://localhost:7474/"));
            Task.Run(async () => await _context.ConnectAsync());
            */

            _context = graphClient;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return new JsonResult(await _context.Cypher.Match("(n:Category)")
                                                       .Where("NOT(n.isArchived)")
                                                       .Return<CategoryDTO>("n").ResultsAsync);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCategory(string name)
        {
            var result = await _context.Cypher.Match("(n:Category)")
                                              .Where("NOT(n.isArchived) AND n.name=~'" + name + "'")
                                              .Return<CategoryDTO>("n").ResultsAsync;
            if (!result.Any()) { return BadRequest("Category doesn't exist"); }

            return new JsonResult(result);
        }

        // POST api/<CategoryController>
        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] string name)
        {
            //await _context.Cypher.Merge("(n:Category {name:'" + name + "', isArchived: false})").ExecuteWithoutResultsAsync();

            var result = await _context.Cypher.Merge("(n:Category {name:'" + name + "', isArchived: false})")
                                              .Return<CategoryDTO>("n")
                                              .ResultsAsync;
            if (!result.Any()) { return BadRequest(); }

            return new JsonResult(result);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("archive/{name}")]
        public async Task Archive(string name)
        {
            await _context.Cypher.Match("(n:Category)")
                                 .Where("NOT(n.isArchived) AND n.name =~ '" + name + "'")
                                 .Set("n.isArchived=" + true)
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<CategoryController>/5
        //[HttpPut("editname/{name}")]
        //public async Task Editname(string name, [FromBody] string newname)
        //{
        //    await _context.Cypher.Match("(n:Category)")
        //                         .Where("NOT(n.isArchived) AND n.name =" + name)
        //                         .Set("n.name=" + newname)
        //                         .ExecuteWithoutResultsAsync();
        //}

        // PUT api/<CategoryController>/5
        [HttpPut("addtag/{categoryname}")]
        public async Task AddTagRelationship(string categoryname, [FromBody] string documentname)
        {
            await _context.Cypher.Match("(n:Category), (d:Document)")
                                 .Where($"NOT(n.isArchived AND d.isArchived) AND n.name =~ '{categoryname}' AND d.name =~ '{documentname}'")
                                 .Merge("(d)-[r: TAG]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("addinterest/{categoryname}")]
        public async Task AddInterestRelationship(string categoryname, [FromBody] string username)
        {
            await _context.Cypher.Match("(n:Category), (d:User)")
                                 .Where($"NOT(n.isArchived AND d.isArchived) AND n.name =~ '{categoryname}' AND d.Username =~ '{username}'")
                                 .Merge("(d)-[r: INTERESTED_IN]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{name}")]
        public async Task Delete(int name)
        {
            await _context.Cypher.Match("(n:Category)")
                                 .Where("n.name =~'" + name + "'")
                                 .DetachDelete("n")
                                 .ExecuteWithoutResultsAsync();
        }
    }
}
