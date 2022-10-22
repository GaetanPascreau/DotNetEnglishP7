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
    [Route("BidList")]
    public class BidListController : Controller
    {
        private readonly ILogger<BidListController> _logger;
        private readonly LocalDbContext _context;

        public BidListController(ILogger<BidListController> logger, LocalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //[HttpGet("/")]
        //public IActionResult Home()
        //{
        //    return View("Home");
        //}

        // GET: /BidList
        [HttpGet("/BidList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CurvePointtDTO>>> GetBidList()
        {
            return await _context.BidList
                .Select(x => BidListToDTO(x))
                .ToListAsync();
        }

        //GET: /BidList/id
       [HttpGet("/BidList/{id}")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CurvePointtDTO>>> GetBidListById(int id)
        {
            return await _context.BidList
                .Where(x => x.BidListId == id)
                .Select(x => BidListToDTO(x))
                .ToListAsync();
        }

        // POST: /BidList
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CurvePointtDTO>> CreateBidList(CurvePointtDTO bidListDTO)
        {
            // ADD FIELDS VALIDATION
            //if (bidListDTO.BidQuantity.GetType() != Decimal)
            //{
            //    return BadRequest();
            //}

            var bidList = new BidList
            {
                Account = bidListDTO.Account,
                Type = bidListDTO.Type,
                BidQuantity = bidListDTO.BidQuantity,
                BidListDate = DateTime.Now,
                CreationDate = DateTime.Now,
                RevisionDate = DateTime.Now
            };

            _context.BidList.Add(bidList);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(
                nameof(GetBidList),
                new { id = bidList.BidListId },
                BidListToDTO(bidList));
        }


        // why is it outside of the Create method ? => move it to CreateBidList ?
        [HttpGet("/bidList/validate")]
        public IActionResult Validate([FromBody]BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return View("bidList/add");
        }

        [HttpGet("/bidList/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            return View("bidList/update");
        }

        ////Original method (duplicated and modified below)
        //[HttpPost("/bidList/update/{id}")]
        //public IActionResult UpdateBid(int id, [FromBody] BidList bidList)
        //{
        //    // TODO: check required fields, if valid call service to update Bid and return list Bid
        //    return Redirect("/bidList/list");
        //}

        //PUT /BidList/id
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBidList(int Id, BidList bidList)
        {
            if(Id != bidList.BidListId)
            {
                return BadRequest();
            }
     
            //bidList.BidListDate = _context.BidList.Find(Id).BidListDate;
            //bidList.CreationDate = _context.BidList.Find(Id).CreationDate;
            //bidList.RevisionDate = _context.BidList.Find(Id).RevisionDate;
            _context.Entry(bidList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!BidListExists(Id))
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

        ////Original method (duplicated and modified below)
        //[HttpDelete("/bidList/{id}")]
        //public IActionResult DeleteBid(int id)
        //{
        //    return Redirect("/bidList/list");
        //}

        //DELETE /BidList/id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BidList>> DeleteBidList(int id)
        {
            var bidList = await _context.BidList.FindAsync(id);
            if (bidList == null)
            {
                return NotFound();
            }
            _context.BidList.Remove(bidList);
            await _context.SaveChangesAsync();
            return bidList;
        }

        private bool BidListExists(int id)
        {
            return _context.BidList.Any(e => e.BidListId == id);
        }

        private static CurvePointtDTO BidListToDTO(BidList bidList) =>
        new CurvePointtDTO
        {
            BidListId = bidList.BidListId,
            Account = bidList.Account,
            Type = bidList.Type,
            BidQuantity = bidList.BidQuantity,
            BidListDate = bidList.BidListDate,
            CreationDate = bidList.CreationDate,
            RevisionDate = bidList.RevisionDate
        };
    }
}