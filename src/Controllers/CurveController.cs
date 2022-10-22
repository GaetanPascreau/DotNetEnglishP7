using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Domain;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("CurvePoint")]
    public class CurveController : Controller
    {
        private readonly ILogger<BidListController> _logger;
        private readonly LocalDbContext _context;

        public CurveController(ILogger<BidListController> logger, LocalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // TODO: Inject Curve Point service

        //[HttpGet("/curvePoint/list")]
        //public IActionResult Home()
        //{
        //    return View("curvePoint/list");
        //}

        // GET: /CurvePoint
        [HttpGet("/CurvePoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CurvePointDTO>>> GetCurvePoint()
        {
            return await _context.CurvePoint
                .Select(x => CurvePointToDTO(x))
                .ToListAsync();
        }

        //GET: /CurvePoint/id
        [HttpGet("/CurvePoint/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CurvePointDTO>>> GetCurvePointById(int id)
        {
            return await _context.CurvePoint
                .Where(x => x.CurveId == id)
                .Select(x => CurvePointToDTO(x))
                .ToListAsync();
        }


        // POST: /CurvePoint
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CurvePointDTO>> CreateCurvePoint(CurvePointDTO curvePointDTO)
        {
            // ADD FIELDS VALIDATION
            var curvePoint = new CurvePoint
            {
                Id = curvePointDTO.Id,
                CurveId = curvePointDTO.CurveId,
                Term = curvePointDTO.Term,
                Value = curvePointDTO.Value,
                AsOfDate = DateTime.Now,
                CreationDate = DateTime.Now
            };

            _context.CurvePoint.Add(curvePoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCurvePoint),
                new { id = curvePoint.Id },
                CurvePointToDTO(curvePoint));
        }

        //PUT /CurvePoint/id
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCurvePoint(int Id, CurvePoint curvePoint)
        {
            if (Id != curvePoint.Id)
            {
                return BadRequest();
            }

            _context.Entry(curvePoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurvePointExists(Id))
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

        //DELETE /CurvePoint/id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurvePoint>> DeleteCurvePoint(int id)
        {
            var curvePoint = await _context.CurvePoint.FindAsync(id);
            if (curvePoint == null)
            {
                return NotFound();
            }
            _context.CurvePoint.Remove(curvePoint);
            await _context.SaveChangesAsync();
            return curvePoint;
        }


        [HttpGet("/curvePoint/add")]
        public IActionResult AddCurvePoint([FromBody]CurvePoint curvePoint)
        {
            return View("curvePoint/add");
        }

        [HttpGet("/curvePoint/add")]
        public IActionResult Validate([FromBody]CurvePoint curvePoint)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return View("curvePoint/add"    );
        }

        [HttpGet("/curvePoint/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            return View("curvepoint/update");
        }

        [HttpPost("/curvepoint/update/{id}")]
        public IActionResult UpdateCurvePointBIS(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            return Redirect("/curvepoint/list");
        }


        private bool CurvePointExists(int id)
        {
            return _context.CurvePoint.Any(e => e.Id == id);
        }

        private static CurvePointDTO CurvePointToDTO(CurvePoint curvePoint) =>
        new CurvePointDTO
        {
            Id = curvePoint.Id,
            CurveId = curvePoint.CurveId,
            Term = curvePoint.Term,
            Value = curvePoint.Value,
            AsOfDate = curvePoint.AsOfDate,
            CreationDate = curvePoint.CreationDate
        };
    }
}