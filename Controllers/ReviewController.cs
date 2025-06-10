using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;
using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CaliCamp.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepo _reviewRepo;

        public ReviewController(IReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        // GET: api/Review
        [HttpGet]
        public IActionResult Get()
        {
            var reviews = _reviewRepo.GetAll();
            return Ok(reviews);
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var review = _reviewRepo.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // POST: api/Review
        [HttpPost]
        public IActionResult Post([FromBody] Review review)
        {
            _reviewRepo.Insert(review);
            return CreatedAtAction(nameof(Get), new { id = review.Id }, review);
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _reviewRepo.Update(review);
            return NoContent();
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingReview = _reviewRepo.GetById(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            _reviewRepo.Delete(id);
            return NoContent();
        }
    }
}
    


