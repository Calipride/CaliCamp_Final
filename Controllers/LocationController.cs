using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;

namespace CaliCamp.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepo _locationRepo;

        public LocationController(ILocationRepo locationRepo)
        {
            _locationRepo = locationRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var locations = _locationRepo.GetAll();
            return Ok(locations);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var location = _locationRepo.GetById(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

       
        [HttpPost]
        public IActionResult Post([FromBody] Location location)
        {
            _locationRepo.Insert(location);
            return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
        }

       
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _locationRepo.Update(location);
            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingLocation = _locationRepo.GetById(id);
            if (existingLocation == null)
            {
                return NotFound();
            }

            _locationRepo.Delete(id);
            return NoContent();
        }
    }

}
