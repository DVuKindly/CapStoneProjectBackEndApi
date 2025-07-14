using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/user/interests")]
    public class InterestsController : ControllerBase
    {
        private readonly UserDbContext _db;

        public InterestsController(UserDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInterests()
        {
            var interests = await _db.Interests
                .Select(i => new { i.Id, i.Name })
                .ToListAsync();

            return Ok(interests);
        }
        [HttpGet("personality")]
        public async Task<ActionResult<List<IdNameDto>>> GetAllPersonalityTraits()
        {
            var traits = await _db.PersonalityTraits
                .Select(t => new IdNameDto { Id = t.Id, Name = t.Name })
                .ToListAsync();

            return Ok(traits);
        }

        [HttpGet("skills")]
        public async Task<ActionResult<List<IdNameDto>>> GetAllSkills()
        {
            var skills = await _db.Skills
                .Select(t => new IdNameDto { Id = t.Id, Name = t.Name })
                .ToListAsync();

            return Ok(skills);
        }
    }


       
    


}
