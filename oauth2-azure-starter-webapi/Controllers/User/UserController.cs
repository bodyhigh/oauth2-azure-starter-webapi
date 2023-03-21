using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oauth2_azure_starter_models.Models.User;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace oauth2_azure_starter_webapi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger logger;

        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
        }

        // GET: api/<UserController>/Profile/291917CA-5F31-4BAB-B3A5-69B41F2B97E6
        [HttpGet("[action]/{id:Guid}")]
        [Produces(MediaTypeNames.Application.Json)]
        public ProfileModel Profile(Guid id)
        {
            this.logger.LogWarning($"TEST WARNING: GET: api/UserController/Profile/{id}");

            return new ProfileModel()
            {
                Id = id,
                UserName = "bob.dobbs",
                FirstName = "bob",
                LastName = "dobbs",
                Email = "bob.dobbs@mail.com",
                Roles = new List<string>{ "User" }
            };
        }

        //// GET: api/<UserController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<UserController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
