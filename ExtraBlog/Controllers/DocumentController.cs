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
                                                       .Return<DocumentDTO>("n").ResultsAsync);
        }

        //GET api/<DocumentController>
        [HttpGet("{name}")]

        public async Task<IActionResult> GetDocument(string name)
        {
            var result = await _context.Cypher.Match("(n:Document)")
                                              .Where("NOT(n.isArchived) AND n.name=~'" + name + "'")
                                              .Return<DocumentDTO>("n").ResultsAsync;
            if (!result.Any()) { return BadRequest("Document doesn't exist"); }

            return new JsonResult(result);
        }
		
		public void parseArgs(ref string[] pomPic, ref string[] pomPar)
		{
			string resPic = "", resPar = "";
			for (int i = 0; i < pomPic.Length; i++)
			{
				resPic += ("'" + pomPic[i] + "'");
			}
			
			for (int i = 0; i < pomPar.Length; i++)
			{
				resPic += ("'" + pomPar[i] + "'");
			}
			
			pomPic = resPic;
			pomPar = resPar;
		}

        //POST api/<DocumentController>
        [HttpPost("add")]
        public async Task<IActionResult> AddDocument([FromBody] DocumentDTO dto)
        {
            //await _context.Cypher.Merge("(n:Document {name: '" + name + "', isArchived: false})").ExecuteWithoutResultsAsync();
			
			string pomPic = dto.Pictures;
			string pomPar = dto.Paragraph;
			parseArgs(ref pomPic, ref pomPar);
            var result = await _context.Cypher.Merge("(n:Document {name:'" + dto.name + "', isArchived: false}, Pictures:[ " + pomPic + "]" + "Paragraphs:[" + pomPar + "]" );
                                              .Return<DocumentDTO>("n")
                                              .ResultsAsync;
            if (!result.Any()) { return BadRequest(); }

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
        //[HttpPut("editname/{name}")]
        //public async Task Editname(string name, [FromBody] string newname)
        //{
        //    await _context.Cypher.Match("(n:Document)")
        //                         .Where("NOT(n.isArchived) AND n.name =" + name)
        //                         .Set("n.name=" + newname)
        //                         .ExecuteWithoutResultsAsync();
        //}

        //PUT api/<DocumentController>
        [HttpPut("addinterest/{documentname}")]

        public async Task AddInterestRelationship(string documentname, [FromBody] string username)
        {
            await _context.Cypher.Match("(n:Document), (d:User)")
                                 .Where($"NOT(n.isArchived AND d.isArchived) AND n.name =~ '{documentname}' AND d.name =~ '{username}'")
                                 .Merge("(d)-[r: INTERESTED_IN]->(n)")
                                 .ExecuteWithoutResultsAsync();
        }

        //DELETE api/<DocumentController>
        [HttpDelete("{name}")]

        public async Task Delete(string name)
        {
            await _context.Cypher.Match("n:Document")
                                 .Where("n.name =~'" + name + "'")
                                 .DetachDelete("n")
                                 .ExecuteWithoutResultsAsync();
        }



    }
}
