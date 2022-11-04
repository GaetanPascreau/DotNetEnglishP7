using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Domain;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("RuleName")]
    public class RuleNameController : Controller
    {
        public readonly ILogger<RuleNameController> _logger;
        public readonly LocalDbContext _context;

        // TODO: Inject RuleName service
        public RuleNameController(ILogger<RuleNameController> logger, LocalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //GET: /RuleName
        [HttpGet("/RuleName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RuleNameDTO>>> GetRuleName()
        {
            return await _context.RuleName
                .Select(x => RuleNameToDTO(x))
                .ToListAsync();
        }

        //GET: /RuleName/id
        [HttpGet("/RuleName/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RuleNameDTO>>> GetRuleNameById(int id)
        {
            if(!RuleNameExists(id))
            {
                return BadRequest("Wrong id.This item does not exist !");
            }
            else
            {
                return await _context.RuleName
                    .Where(r => r.Id == id)
                    .Select(x => RuleNameToDTO(x))
                    .ToListAsync();
            }  
        }

        //POST: /RuleName
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RuleNameDTO>> CreateRuleName(RuleNameDTO ruleNameDTO)
        {
            var ruleName = new RuleName
            {
                Name = ruleNameDTO.Name,
                Description = ruleNameDTO.Description
            };

            _context.Add(ruleName);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRuleName),
                new { Id = ruleName.Id },
                RuleNameToDTO(ruleName));
        }

        //PUT: /RuleName/id
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRuleName(int Id, RuleName ruleName)
        {
            if(Id != ruleName.Id)
            {
                return BadRequest();
            }

            _context.Entry(ruleName).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuleNameExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //DELETE: /RuleName/id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RuleName>> DeleteRuleName(int id)
        {
            var ruleName = await _context.RuleName.FindAsync(id);
            if (ruleName == null)
            {
                return NotFound();
            }

            _context.RuleName.Remove(ruleName);
            await _context.SaveChangesAsync();

            return ruleName;
        }

         // original code below
        [HttpGet("/ruleName/list")]
        public IActionResult Home()
        {
            // TODO: find all RuleName, add to model
            return View("ruleName/list");
        }

        [HttpGet("/ruleName/add")]
        public IActionResult AddRuleName([FromBody]RuleName trade)
        {
            return View("ruleName/add");
        }

        [HttpGet("/ruleName/add")]
        public IActionResult Validate([FromBody]RuleName trade)
        {
            // TODO: check data valid and save to db, after saving return RuleName list
            return View("ruleName/add");
        }

        [HttpGet("/ruleName/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            return View("ruleName/update");
        }

        [HttpPost("/ruleName/update/{id}")]
        public IActionResult updateRuleName(int id, [FromBody] RuleName rating)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            return Redirect("/ruleName/list");
        }

        //[HttpDelete("/ruleName/{id}")]
        //public IActionResult DeleteRuleNameBIS(int id)
        //{
        //    // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
        //    return Redirect("/ruleName/list");
        //}

        private bool RuleNameExists(int id)
        {
            return _context.RuleName.Any(r => r.Id == id);
        }

        private static RuleNameDTO RuleNameToDTO(RuleName ruleName) =>
            new RuleNameDTO
            {
                Id = ruleName.Id,
                Name = ruleName.Name,
                Description = ruleName.Description
            };
        
    }
}