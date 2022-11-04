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
 
namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("Rating")]
    public class RatingController : Controller
    {
        private readonly ILogger<RatingController> _logger;
        private readonly LocalDbContext _context;
        // TODO: Inject Rating service

        public RatingController(ILogger<RatingController> logger, LocalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //GET /Rating
        [HttpGet("/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Rating>> GetRating()
        {
            return _context.Rating.ToList();
        }

        //GET /Rating/id
        [HttpGet("/rating/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Rating>> GetRatingById(int id)
        {
            if(!RatingExists(id))
            {
                return BadRequest("Wrong id. This item does not exist !");
            }
            else
            {
                return _context.Rating.Where(x => x.Id == id).ToList();
            }
            
        }

        //POST /Rating
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Rating>> CreateRating(Rating rating)
        {
            var NewRating = new Rating
            {
                MoodysRating = rating.MoodysRating,
                SandPRating = rating.SandPRating,
                FitchRating = rating.FitchRating,
                OrderNumber = rating.OrderNumber
            };

            _context.Add(NewRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRating),
                new { Id = rating.Id },
                rating);
        }

        //PUT /Rating/id
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRating(int Id, Rating rating)
        {
            if (Id != rating.Id)
            {
                return BadRequest();
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(Id))
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

        //DELETE /Rating/id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Rating>> DeleteRating(int Id)
        {
            var rating = await _context.Rating.FindAsync(Id);
            if(rating == null)
            {
                return NotFound();
            }
            _context.Rating.Remove(rating);
            await _context.SaveChangesAsync();
            return rating;
        }



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

        // ORIGINAL CODE
        //[HttpGet("/rating/update/{id}")]
        //public IActionResult ShowUpdateForm(int id)
        //{
        //    // TODO: get Rating by Id and to model then show to the form
        //    return View("rating/update");
        //}

        //[HttpPost("/rating/update/{id}")]
        //public IActionResult updateRating(int id, [FromBody] Rating rating)
        //{
        //    // TODO: check required fields, if valid call service to update Rating and return Rating list
        //    return Redirect("/rating/list");
        //}

        //[HttpDelete("/rating/{id}")]
        //public IActionResult DeleteRatingBIS(int id)
        //{
        //    // TODO: Find Rating by Id and delete the Rating, return to Rating list
        //    return Redirect("/rating/list");
        //}

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.Id == id);
        }
    }
}