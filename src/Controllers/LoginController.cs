using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("login");
        }

        [HttpGet("/secure/article-details")]
        public IActionResult GetAllUserArticles()
        {
            return View(_userRepository.FindAll());
        }

        [HttpGet("/error")]
        public IActionResult Error()
        {
            string errorMessage= "You are not authorized for the requested data.";
            
            return View(new UnauthorizedObjectResult(errorMessage));
        }

        [HttpGet("/trade/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            return View("trade/update");
        }

        [HttpPost("/trade/update/{id}")]
        public IActionResult updateTrade(int id, [FromBody] Trade rating)
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
    }
}