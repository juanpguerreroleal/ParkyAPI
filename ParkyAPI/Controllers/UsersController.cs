using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DataAccess.Repository.IRepository;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
        }
        /// <summary>
        /// Authenticate user method
        /// </summary>
        /// <param name="model">The user model with username and password</param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User model)
        {
            var user = _userRepository.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }
    }
}
