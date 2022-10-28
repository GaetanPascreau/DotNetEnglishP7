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
    [Route("Trade")]
    public class TradeController : Controller
    {
        public readonly ILogger<RuleNameController> _logger;
        public readonly LocalDbContext _context;

        // TODO: Inject Trade service

        //GET: /Trade
        [HttpGet("/Trade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TradeDTO>>> GetTrade()
        {
            return await _context.Trade
                .Select(x => TradeToDTO(x))
                .ToListAsync();
        }

        //GET: />Trade/id
        [HttpGet("/Trade/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TradeDTO>>> GetTradeById(int id)
        {
            return await _context.Trade
                .Where(t => t.TradeId == id)
                .Select(x => TradeToDTO(x))
                .ToListAsync();
        }

        //POST: /Trade
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TradeDTO>> CreateTrade(TradeDTO tradeDTO)
        {
            var trade = new Trade
            {
                Account = tradeDTO.Account,
                Type = tradeDTO.Type,
                BuyQuantity = !string.IsNullOrWhiteSpace((tradeDTO.BuyQuantity).ToString()) ? (Decimal?)Decimal.Parse((tradeDTO.BuyQuantity).ToString()) : null,
                SellQuantity = !string.IsNullOrWhiteSpace((tradeDTO.SellQuantity).ToString()) ? (Decimal?)Decimal.Parse((tradeDTO.SellQuantity).ToString()) : null,
                BuyPrice = !string.IsNullOrWhiteSpace((tradeDTO.BuyPrice).ToString()) ? (Decimal?)Decimal.Parse((tradeDTO.BuyPrice).ToString()) : null,
                SellPrice = !string.IsNullOrWhiteSpace((tradeDTO.SellPrice).ToString()) ? (Decimal?)Decimal.Parse((tradeDTO.BuyQuantity).ToString()) : null,
                TradeDate = DateTime.Now,
                CreationDate = DateTime.Now,
                RevisionDate = DateTime.Now
            };

            _context.Add(trade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(trade),
                new { TradeId = trade.TradeId },
                TradeToDTO(trade));
        }

        //PUT: /Trade/id
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTrade(int TradeId, Trade trade)
        {
            if (TradeId != trade.TradeId)
            {
                return BadRequest();
            }

            _context.Entry(trade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeExists(TradeId))
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

        //DELETE: /Trade/id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Trade>> DeleteTrade(int TradeId)
        {
            var trade = await _context.Trade.FindAsync(TradeId);
            if (trade == null)
            {
                return NotFound();
            }

            _context.Trade.Remove(trade);
            await _context.SaveChangesAsync();

            return trade;
        }



        // Original code below
        [HttpGet("/trade/list")]
        public IActionResult Home()
        {
            // TODO: find all Trade, add to model
            //var alltrades = _context.Trade.ToList();
            return View("trade/list");
        }

        [HttpGet("/trade/add")]
        public IActionResult AddTrade([FromBody]Trade trade)
        {
            return View("trade/add");
        }

        [HttpGet("/trade/add")]
        public IActionResult Validate([FromBody]Trade trade)
        {
            // TODO: check data valid and save to db, after saving return Trade list
            return View("trade/add");
        }

        [HttpGet("/trade/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            return View("trade/update");
        }

        [HttpPost("/trade/update/{id}")]
        public IActionResult updateTrade(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            return Redirect("/trade/list");
        }

        //[HttpDelete("/trade/{id}")]
        //public IActionResult DeleteTrade(int id)
        //{
        //    // TODO: Find Trade by Id and delete the Trade, return to Trade list
        //    return Redirect("/trade/list");
        //}

        private bool TradeExists(int id)
        {
            return _context.Trade.Any(t => t.TradeId == id);
        }

        private static TradeDTO TradeToDTO(Trade trade) =>
            new TradeDTO
            {
                TradeId = trade.TradeId,
                Account = trade.Account,
                Type = trade.Type,
                BuyQuantity = !string.IsNullOrWhiteSpace((trade.BuyQuantity).ToString()) ? (Decimal?)Decimal.Parse((trade.BuyQuantity).ToString()) : null,
                SellQuantity = !string.IsNullOrWhiteSpace((trade.SellQuantity).ToString()) ? (Decimal?)Decimal.Parse((trade.SellQuantity).ToString()) : null,
                BuyPrice = !string.IsNullOrWhiteSpace((trade.BuyPrice).ToString()) ? (Decimal?)Decimal.Parse((trade.BuyPrice).ToString()) : null,
                SellPrice = !string.IsNullOrWhiteSpace((trade.SellPrice).ToString()) ? (Decimal?)Decimal.Parse((trade.BuyQuantity).ToString()) : null,
                TradeDate = trade.TradeDate,
                CreationDate = trade.CreationDate,
                RevisionDate = trade.RevisionDate
            };
    }
}