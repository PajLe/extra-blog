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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _context.Cypher.Match("(n:Category)")
                                              .Where("NOT(n.isArchived) AND id(n)=" + id)
                                              .Return<CategoryDTO>("n").ResultsAsync;
            if (result == null) { return BadRequest("Category doesn't exist"); }

            return new JsonResult(result);
        }

        // POST api/<CategoryController>
        [HttpPost("add")]
        public async Task AddCategory([FromBody] string name)
        {
            await _context.Cypher.Merge("(n:Category {name:'" + name + "', isArchived: false})").ExecuteWithoutResultsAsync();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("archive/{id}")]
        public async Task Archive(int id)
        {
            await _context.Cypher.Match("(n:Category)")
                                 .Where("NOT(n.isArchived) AND id(n) =" + id)
                                 .Set("n.isArchived=" + true)
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("editname/{id}")]
        public async Task EditName(int id, [FromBody] string newName)
        {
            await _context.Cypher.Match("(n:Category)")
                                 .Where("NOT(n.isArchived) AND id(n) =" + id)
                                 .Set("n.name=" + newName)
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("addtag/{categoryid}")]
        public async Task AddTagRelationship(int categoryId, [FromBody] int documentId)
        {
            await _context.Cypher.Match("(n:Category), (d:Document)")
                                 .Where($"NOT(n.isArchived AND d.isArchived) AND id(n) = {categoryId} AND id(d) = {documentId}")
                                 .Merge("(d)-[r: TAG]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("addinterest/{categoryid}")]
        public async Task AddInterestRelationship(int categoryId, [FromBody] int userId)
        {
            await _context.Cypher.Match("(n:Category), (d:User)")
                                 .Where($"NOT(n.isArchived AND d.isArchived) AND id(n) = {categoryId} AND id(d) = {userId}")
                                 .Merge("(d)-[r: INTERESTED_IN]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _context.Cypher.Match("(n:Category)")
                                 .Where("id(n) =" + id)
                                 .DetachDelete("n")
                                 .ExecuteWithoutResultsAsync();
        }
    }
}
