using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;
using Microsoft.Extensions.Logging;

namespace CaliCamp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IUserRepo _userRepo;
        private readonly ICampingSpotRepo _campingSpotRepo;
        private readonly ILogger<BookingController> _logger;

        public BookingController(
            IBookingRepo bookingRepo,
            IUserRepo userRepo,
            ICampingSpotRepo campingSpotRepo,
            ILogger<BookingController> logger)
        {
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _campingSpotRepo = campingSpotRepo;
            _logger = logger;
        }

        // GET: api/Booking
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Booking>> Get()
        {
            try
            {
                var bookings = _bookingRepo.GetAll();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all bookings");
                return StatusCode(500, "An error occurred while retrieving bookings");
            }
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Booking> Get(int id)
        {
            try
            {
                var booking = _bookingRepo.GetById(id);
                if (booking == null)
                {
                    return NotFound();
                }
                return Ok(booking);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the booking");
            }
        }

        // POST: api/Booking
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Booking> Post([FromBody] Booking booking)
        {
            try
            {
                if (booking == null)
                {
                    return BadRequest("Booking cannot be null");
                }

                if (booking.StartDate >= booking.EndDate)
                {
                    return BadRequest("End date must be after start date");
                }

                if (booking.NumGuests <= 0)
                {
                    return BadRequest("Number of guests must be greater than 0");
                }

                // Check if user exists
                var user = _userRepo.GetById(booking.UserId);
                if (user == null)
                {
                    return NotFound($"User with ID {booking.UserId} not found");
                }

                // Check if camping spot exists and validate capacity
                var campingSpot = _campingSpotRepo.GetById(booking.CampingSpotId);
                if (campingSpot == null)
                {
                    return NotFound($"Camping spot with ID {booking.CampingSpotId} not found");
                }

                if (booking.NumGuests > campingSpot.MaxGuests)
                {
                    return BadRequest($"Number of guests ({booking.NumGuests}) exceeds camping spot capacity ({campingSpot.MaxGuests})");
                }

                // Check for date conflicts with existing bookings
                var existingBookings = _bookingRepo.GetAll()
                    .Where(b => b.CampingSpotId == booking.CampingSpotId)
                    .Where(b => !(b.EndDate <= booking.StartDate || b.StartDate >= booking.EndDate))
                    .ToList();

                if (existingBookings.Any())
                {
                    return BadRequest("The camping spot is already booked for the selected dates");
                }

                _bookingRepo.Insert(booking);
                return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking");
                return StatusCode(500, "An error occurred while creating the booking");
            }
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put(int id, [FromBody] Booking booking)
        {
            try
            {
                if (booking == null || id != booking.Id)
                {
                    return BadRequest("Invalid booking data");
                }

                if (booking.StartDate >= booking.EndDate)
                {
                    return BadRequest("End date must be after start date");
                }

                // Check if user exists
                var user = _userRepo.GetById(booking.UserId);
                if (user == null)
                {
                    return NotFound($"User with ID {booking.UserId} not found");
                }

                // Check if camping spot exists and validate capacity
                var campingSpot = _campingSpotRepo.GetById(booking.CampingSpotId);
                if (campingSpot == null)
                {
                    return NotFound($"Camping spot with ID {booking.CampingSpotId} not found");
                }

                if (booking.NumGuests > campingSpot.MaxGuests)
                {
                    return BadRequest($"Number of guests ({booking.NumGuests}) exceeds camping spot capacity ({campingSpot.MaxGuests})");
                }

                // Check for date conflicts with existing bookings
                var existingBookings = _bookingRepo.GetAll()
                    .Where(b => b.CampingSpotId == booking.CampingSpotId && b.Id != booking.Id)
                    .Where(b => !(b.EndDate <= booking.StartDate || b.StartDate >= booking.EndDate))
                    .ToList();

                if (existingBookings.Any())
                {
                    return BadRequest("The camping spot is already booked for the selected dates");
                }

                _bookingRepo.Update(booking);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the booking");
            }
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            try
            {
                _bookingRepo.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting booking with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the booking");
            }
        }
    }
}
