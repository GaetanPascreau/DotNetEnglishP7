using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Domain;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        private UserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly LocalDbContext _context;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        //GET: /User
        [HttpGet("/User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            return await _context.Users
                .Select(x => UserToDTO(x))
                .ToListAsync();
        }

        //GET: />User/id
        [HttpGet("/User/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserById(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(x => UserToDTO(x))
                .ToListAsync();
        }

        //POST: /User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userDTO)
        {
            var user = new User
            {
                UserName = userDTO.UserName,
                Password = userDTO.Password,
                FullName = userDTO.FullName
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(user),
                new { Id = user.Id },
               UserToDTO(user));
        }

        //PUT: /User/id
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int Id, User user)
        {
            if (Id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(Id))
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

        //DELETE: /User/id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> DeleteUser(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }


        // Original code
        [HttpGet("/user/list")]
        public IActionResult Home()
        {
            return View(_userRepository.FindAll());
        }

        [HttpGet("/user/add")]
        public IActionResult AddUser([FromBody]User user)
        {
            return View("user/add");
        }

        [HttpGet("/user/validate")]
        public IActionResult Validate([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("user/add");
            }
           
           _userRepository.Add(user);
           
            return Redirect("user/list");
        }

        [HttpGet("/user/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            User user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);
            
            return View("user/update");
        }

        [HttpPost("/user/update/{id}")]
        public IActionResult updateUser(int id, [FromBody] User user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            return Redirect("/trade/list");
        }

        //[HttpDelete("/user/{id}")]
        //public IActionResult DeleteUser(int id)
        //{
        //    User user = _userRepository.FindById(id);
            
        //    if (user == null)
        //        throw new ArgumentException("Invalid user Id:" + id);
                        
        //    return Redirect("/user/list");
        //}

        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        private static UserDTO UserToDTO(User user) =>
            new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                FullName = user.FullName 
            };
    }
}