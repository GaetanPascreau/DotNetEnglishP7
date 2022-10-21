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
    public class RatingController : Controller
    {
        // TODO: Inject Rating service

        [HttpGet("/rating/list")]
        public IActionResult Home()
        {
            // TODO: find all Rating, add to model
            return View("rating/list");
        }

        [HttpGet("/rating/add")]
        public IActionResult AddRatingForm([FromBody]Rating rating)
        {
            return View("rating/add");
        }

        [HttpGet("/rating/add")]
        public IActionResult Validate([FromBody]Rating rating)
        {
            // TODO: check data valid and save to db, after saving return Rating list
            return View("rating/add");
        }

        [HttpGet("/rating/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            return View("rating/update");
        }

        [HttpPost("/rating/update/{id}")]
        public IActionResult updateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            return Redirect("/rating/list");
        }

        [HttpDelete("/rating/{id}")]
        public IActionResult DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            return Redirect("/rating/list");
        }
    }
}