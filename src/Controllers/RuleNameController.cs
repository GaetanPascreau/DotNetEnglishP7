using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleNameController : Controller
    {
        // TODO: Inject RuleName service

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

        [HttpDelete("/ruleName/{id}")]
        public IActionResult DeleteRuleName(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            return Redirect("/ruleName/list");
        }
    }
}