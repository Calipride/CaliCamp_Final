using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;
using CaliCamp.Models;

namespace CaliCamp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmenitysController : ControllerBase
    {
        private readonly IAmenityRepo _amenityRepo;

        public AmenitysController(IAmenityRepo amenityRepo)
        {
            _amenityRepo = amenityRepo;
        }

        // GET: api/Amenity
        [HttpGet]
        public IActionResult Get()
        {
            var amenities = _amenityRepo.GetAll();
            return Ok(amenities);
        }

        // GET: api/Amenity/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var amenity = _amenityRepo.GetById(id);
            if (amenity == null)
            {
                return NotFound();
            }
            return Ok(amenity);
        }

        // POST: api/Amenity
        [HttpPost]
        public IActionResult Post([FromBody] Amenity amenity)
        {
            _amenityRepo.Insert(amenity);
            return CreatedAtAction(nameof(Get), new { id = amenity.Id }, amenity);
        }

        // PUT: api/Amenity/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Amenity amenity)
        {
            if (id != amenity.Id)
            {
                return BadRequest();
            }

            _amenityRepo.Update(amenity);
            return NoContent();
        }

        // DELETE: api/Amenity/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingAmenity = _amenityRepo.GetById(id);
            if (existingAmenity == null)
            {
                return NotFound();
            }

            _amenityRepo.Delete(id);
            return NoContent();
        }


    }
}
