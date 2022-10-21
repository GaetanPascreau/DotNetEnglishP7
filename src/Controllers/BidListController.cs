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

        //// GET /bidList/list
        //[HttpGet("/bidList/list")]
        //public IQueryable<BidList> GetBidList()
        //{
        //    var bids = from b in _context.BidList
        //               select new BidList()
        //               {
        //                   BidListId = b.BidListId,
        //                   Account = b.Account,
        //                   Type = b.Type,
        //                   BidQuantity = b.BidQuantity
        //               };

        //    return bids;
        //}

        // GET: /BidList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidListDTO>>> GetBidList()
        {
            return await _context.BidList
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // POST: /BidList
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BidListDTO>> CreateBidList(BidListDTO bidListDTO)
        {
            var bidList = new BidList
            {
                Account = bidListDTO.Account,
                Type = bidListDTO.Type,
                BidQuantity = bidListDTO.BidQuantity
            };

            _context.BidList.Add(bidList);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(
                nameof(GetBidList),
                new { id = bidList.BidListId },
                ItemToDTO(bidList));
        }



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

        [HttpPost("/bidList/update/{id}")]
        public IActionResult UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            return Redirect("/bidList/list");
        }

        [HttpDelete("/bidList/{id}")]
        public IActionResult DeleteBid(int id)
        {
            return Redirect("/bidList/list");
        }

        private static BidListDTO ItemToDTO(BidList bidList) =>
        new BidListDTO
        {
            BidListId = bidList.BidListId,
            Account = bidList.Account,
            Type = bidList.Type,
            BidQuantity = bidList.BidQuantity
        };
    }
}