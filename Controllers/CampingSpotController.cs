using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;
using CaliCamp.Models;

namespace CaliCamp.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class CampingSpotController : ControllerBase
    {
        private readonly ICampingSpotRepo _campingSpotRepo;


        public CampingSpotController(ICampingSpotRepo campingSpotRepo)
        {
            _campingSpotRepo = campingSpotRepo;
        }

        // GET: api/CampingSpot
        [HttpGet]
        public IActionResult Get()
        {
            var campingSpots = _campingSpotRepo.GetAll();
            return Ok(campingSpots);
        }

        // GET: api/CampingSpot/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var campingSpot = _campingSpotRepo.GetById(id);
            if (campingSpot == null)
            {
                return NotFound();
            }
            return Ok(campingSpot);
        }

        // POST: api/CampingSpot
        [HttpPost]
        public IActionResult Post([FromBody] CampingSpot campingSpot)
        {
            _campingSpotRepo.Insert(campingSpot);
            return CreatedAtAction(nameof(Get), new { id = campingSpot.Id }, campingSpot);
        }

        // PUT: api/CampingSpot/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CampingSpot campingSpot)
        {
            if (id != campingSpot.Id)
            {
                return BadRequest();
            }

            _campingSpotRepo.Update(campingSpot);
            return NoContent();
        }

        // DELETE: api/CampingSpot/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCampingSpot = _campingSpotRepo.GetById(id);
            if (existingCampingSpot == null)
            {
                return NotFound();
            }

            _campingSpotRepo.Delete(id);
            return NoContent();
        }


        // GET: api/CampingSpot/User/5
        [HttpGet("User/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var campingSpots = _campingSpotRepo.GetByUserId(userId);
            if (campingSpots == null || !campingSpots.Any())
            {
                return NotFound();
            }
            return Ok(campingSpots);
        }
    }
}

