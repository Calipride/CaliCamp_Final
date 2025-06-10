using CaliCamp.DataAccess;
using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using CaliCamp.Helpers;
using CaliCamp.Models.Dtos;

namespace CaliCamp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly PasswordHelper _passwordHelper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserController(IUserRepo userRepo, PasswordHelper passwordHelper, IWebHostEnvironment hostingEnvironment)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _passwordHelper = passwordHelper ?? throw new ArgumentNullException(nameof(passwordHelper));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        // GET
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepo.GetAll();
            return Ok(users);
        }

    
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null || string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var existingUser = _userRepo.GetByEmail(registerDto.Email);
            if (existingUser != null)
            {
                return Conflict("Email address already exists");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordHelper.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Bookings = new List<Booking>(),
                Reviews = new List<Review>(),
            };

            _userRepo.Insert(user);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var user = _userRepo.GetByEmail(loginDto.Email);
            if (user == null || !_passwordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // I added my JWT token below


            return Ok(new { message = "Login successful" });
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userRepo.Update(user);
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingUser = _userRepo.GetById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userRepo.Delete(id);
            return NoContent();
        }

        // GET
        [HttpGet("emails-passwords")]
        public IActionResult GetAllEmailsAndPasswords()
        {
            var users = _userRepo.GetAll()
                                       .Select(user => new
                                       {
                                           user.Id,
                                           user.Username,
                                           user.Email,
                                           user.PasswordHash,
                                           user.FirstName,
                                           user.LastName,
                                           user.PhoneNumber,
                                           user.ProfilePicturePath
                                       })
                                       .ToList();

            return Ok(users);
        }

       
        [HttpGet("admins")]
        public IActionResult GetAdminUsers()
        {
            var adminUsers = _userRepo.GetAll().Where(u => u.IsAdmin);
            return Ok(adminUsers);
        }

        // POST
        [HttpPost("{id}/upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Invalid file." });
            }

            var user = _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                var existingImagePath = Path.Combine(_hostingEnvironment.WebRootPath, user.ProfilePicturePath);
                if (System.IO.File.Exists(existingImagePath))
                {
                    System.IO.File.Delete(existingImagePath);
                }
            }

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "User_images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create a unique file name
            var uniqueFileName = $"{user.Id}_{Path.GetRandomFileName()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            user.ProfilePicturePath = $"User_images/{uniqueFileName}";
            _userRepo.Update(user);

            return Ok(new { path = user.ProfilePicturePath });
        }
    }
}