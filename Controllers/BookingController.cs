using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;


namespace CaliCamp.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepo;

        public BookingsController(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        // GET: api
        [HttpGet]
        public IActionResult Get()
        {
            var bookings = _bookingRepo.GetAll();
            return Ok(bookings);
        }

        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var booking = _bookingRepo.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        
        [HttpPost]
        public IActionResult Post([FromBody] Booking booking)
        {
            if (booking == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            _bookingRepo.Insert(booking);
            return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
        }

       
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _bookingRepo.Update(booking);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingBooking = _bookingRepo.GetById(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            _bookingRepo.Delete(id);
            return NoContent();
        }


    }

}
