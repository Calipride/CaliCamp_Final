using Microsoft.AspNetCore.Mvc;
using CaliCamp.DataAccess;
using CaliCamp.Models;

namespace CaliCamp.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepo _paymentRepo;

        public PaymentController(IPaymentRepo paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        // GET: api/Payment
        [HttpGet]
        public IActionResult Get()
        {
            var payments = _paymentRepo.GetAll();
            return Ok(payments);
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var payment = _paymentRepo.GetById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // POST: api/Payment
        [HttpPost]
        public IActionResult Post([FromBody] Payment payment)
        {
            _paymentRepo.Insert(payment);
            return CreatedAtAction(nameof(Get), new { id = payment.Id }, payment);
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _paymentRepo.Update(payment);
            return NoContent();
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingPayment = _paymentRepo.GetById(id);
            if (existingPayment == null)
            {
                return NotFound();
            }

            _paymentRepo.Delete(id);
            return NoContent();
        }
    }
}

