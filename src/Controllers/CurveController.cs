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
    [Route("curvePoint")]
    public class CurveController : Controller
    {
        // TODO: Inject Curve Point service
        private readonly ILogger<CurveController> _logger;
        private readonly LocalDbContext _context;
 
        public CurveController(ILogger<CurveController> logger, LocalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /curvePoint/list
        [HttpGet("/curvePoint/list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CurvePointDTO>>> GetCurvePoint()
        {
            _logger.LogInformation("User requested the list of curvePoints");
            Console.WriteLine(_logger);
            return await _context.CurvePoint
                .Select(x => CurvePointToDTO(x))
                .ToListAsync();
        }

        //GET: /curvePoint/id
        [HttpGet("/curvePoint/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CurvePointDTO>>> GetCurvePointById(int id)
        {
            if(!CurvePointExists(id))
            {
                return (BadRequest("Wrong id. This item does not exists !"));
            }
            else
            {
                return await _context.CurvePoint
                    .Where(c => c.Id == id)
                    .Select(x => CurvePointToDTO(x))
                    .ToListAsync();
            } 
        }

        // POST: /curvePoint
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

        //PUT: /curvePoint/id
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

        //DELETE: /curvePoint/id
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

        //ORIGINAL CODE below :

        //[HttpGet("/curvePoint/list")]
        //public IActionResult Home()
        //{
        //    return View("curvePoint/list");
        //}

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
            return _context.CurvePoint.Any(c => c.Id == id);
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