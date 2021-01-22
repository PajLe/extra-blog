using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtraBlog.DTOs;
using ExtraBlog.Models;
using Neo4jClient;

namespace ExtraBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IGraphClient _context;

        public DocumentController(IGraphClient graphClient)
        {
            /*
            _context = new GraphClient(new Uri("http://localhost:7474/"));
            Task.Run(async () => await _context.ConnectAsync());
            */

            _context = graphClient;
        }

        //GET api/<DocumentController>
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            return new JsonResult(await _context.Cypher.Match("(n:Document)")
                                                       .Where("NOT(n.isArchived)")
                                                       .Return<Document>("n").ResultsAsync);
        }

        //GET api/<DocumentController>
        [HttpGet("{name}")]

        public async Task<IActionResult> GetDocument(string name)
        {
            var result = await _context.Cypher.Match("(n:Document)")
                                              .Where("NOT(n.isArchived) AND n.name=~'" + name + "'")
                                              .Return<Document>("n").ResultsAsync;

            return new JsonResult(result);
        }

        [HttpGet("my/{name}")]

        public async Task<IActionResult> GetMyDocuments(string name)
        {
            var result = await _context.Cypher.Match("(n:Document)")
                                              .Where("NOT(n.isArchived) AND exists(n.CreatedBy) AND n.CreatedBy=~'" + name + "'")
                                              .Return<Document>("n").ResultsAsync;

            return new JsonResult(result);
        }

        //POST api/<DocumentController>
        [HttpPost("add")]
        public async Task<IActionResult> AddDocument([FromBody] DocumentDTO dto)
        {
            //await _context.Cypher.Merge("(n:Document {name: '" + name + "', isArchived: false})").ExecuteWithoutResultsAsync();

            string[] pomPic = dto.Pictures;
            string[] pomPar = dto.Paragraphs;
            parseArgs(pomPic, pomPar, out string pic, out string par);
            string query = "(n:Document {name:'" + dto.name + "', isArchived: false, Pictures:[ " + pic + "], Paragraphs:[" + par + "], CreatedBy:'" + dto.CreatedBy + "', Created: date()})";
            var result = await _context.Cypher.Merge(query)
                                              .Return<Document>("n")
                                              .ResultsAsync;

            parseArg(dto.Categories, out string categories);
            await _context.Cypher.Match("(d:Document), (c:Category)")
                    .Where($"NOT(d.isArchived AND c.isArchived) AND d.name =~ '{dto.name}' AND c.name IN [{categories}]")
                    .Merge("(d)-[r: TAG]->(c)")
                    .ExecuteWithoutResultsAsync();

            return new JsonResult(result);
        }

        // PUT api/<DocumentController>
        [HttpPut("archive/{name}")]
        public async Task Archive(string name)
        {
            await _context.Cypher.Match("(n:Document)")
                                 .Where("NOT(n.isArchived) AND n.name =~ '" + name + "'")
                                 .Set("n.isArchived=" + true)
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<DocumentController>
        [HttpPut("editname/{name}")]
        public async Task Editname(string name, [FromBody] string newname)
        {
            await _context.Cypher.Match("(n:Document)")
                                 .Where("NOT(n.isArchived) AND n.name =" + name)
                                 .Set("n.name=" + newname)
                                 .ExecuteWithoutResultsAsync();
        }

        // PUT api/<DocumentController>
        [HttpPut("edit/{name}")]
        public async Task EditDocument(string name, [FromBody] DocumentEditDTO edit)
        {
            parseArg(edit.Paragraphs, out string par);
            parseArg(edit.Pictures, out string pic);
            await _context.Cypher.Match("(n:Document)")
                                 .Where("NOT(n.isArchived) AND n.name =~ '" + name + "'")
                                 .Set("n.Paragraphs=[" + par + "]")
                                 .Set("n.Pictures=[" + pic + "]")
                                 .ExecuteWithoutResultsAsync();
        }

        //PUT api/<DocumentController>
        [HttpPut("addlike/{documentname}")]

        public async Task AddLikeRelationship(string documentname, [FromBody] string username)
        {
            await _context.Cypher.Match("(n:Document), (d:User)")
                                 .Where($"NOT(n.isArchived AND d.isArchived) AND n.name =~ '{documentname}' AND d.Username =~ '{username}'")
                                 .Merge("(d)-[r: LIKES]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        [HttpGet("likes/{name}")]
        public async Task<IActionResult> GetDocumentLikes(string name)
        {
            var result = await _context.Cypher.Match($"(n:Document {{name: '{name}'}})<-[r:LIKES]-(u:User)")
                                              .Return<int>("count(*)").ResultsAsync;

            return new JsonResult(result);
        }

        //PUT api/<DocumentController>
        [HttpPut("tags/{documentname}")]

        public async Task AddTags(string documentname, [FromBody] string[] category)
        {
            parseArg(category, out string categories);
            await _context.Cypher.Match("(d:Document), (c:Category)")
                                 .Where($"NOT(d.isArchived AND c.isArchived) AND d.name =~ '{documentname}' AND c.name IN [{categories}]")
                                 .Merge("(d)-[r: TAG]->(c)")
                                 .ExecuteWithoutResultsAsync();
        }

        //DELETE api/<DocumentController>
        [HttpDelete("{name}")]

        public async Task Delete(string name)
        {
            await _context.Cypher.Match("(n:Document)")
                                 .Where("n.name =~'" + name + "'")
                                 .DetachDelete("n")
                                 .ExecuteWithoutResultsAsync();
        }

        [HttpPut("addseen/{documentname}/{username}")]

        public async Task AddSeenRelationship(string documentname, string username)
        {
            await _context.Cypher.Match("(n:Document{name:'" + documentname + "'}), (d:User{Username:'" + username + "'})")
                                 .Where("NOT(n.isArchived)")
                                 .Merge("(d)-[r: SEEN]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        [HttpGet("seens/{name}")]
        public async Task<IActionResult> GetDocumentSeens(string name)
        {
            var result = await _context.Cypher.Match($"(n:Document {{name: '{name}'}})<-[r:SEEN]-(u:User)")
                                              .Return<int>("count(*)").ResultsAsync;
            if (!result.Any()) { return BadRequest("Document doesn't exist"); }

            return new JsonResult(result);
        }

        private void parseArgs(string[] pomPic, string[] pomPar, out string pic, out string par)
        {
            string resPic = "", resPar = "";
            for (int i = 0; i < pomPic.Length; i++)
            {
                if (i != 0) { resPic += ","; }
                resPic += " ";
                resPic += ("'" + pomPic[i] + "'");
            }

            for (int i = 0; i < pomPar.Length; i++)
            {
                if (i != 0) { resPar += ","; }
                resPar += " ";
                resPar += ("'" + pomPar[i] + "'");
            }

            pic = resPic;
            par = resPar;
        }

        private void parseArg(string[] pom, out string res)
        {
            string resPic = "";
            for (int i = 0; i < pom.Length; i++)
            {
                if (i != 0) { resPic += ","; }
                resPic += " ";
                resPic += ("'" + pom[i] + "'");
            }

            res = resPic;
        }

    }
}
