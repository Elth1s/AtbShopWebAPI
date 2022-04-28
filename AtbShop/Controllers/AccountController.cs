using AtbShop.Helpers;
using AtbShop.Models;
using AtbShop.Services;
using AutoMapper;
using DAL.Data;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;

namespace AtbShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<AppUser> _userManager;
        //private readonly ILogger<AccountController> _logger;
        private readonly AppEFContext _context;
        public AccountController(UserManager<AppUser> userManager,
            IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        /// <summary>
        /// Авторизація на сайті
        /// </summary>
        /// <param name="model">Логін та пароль користувача</param>
        /// <returns>Повертає токен авторизації</returns>

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        return Ok(new { token = await _jwtTokenService.CreateTokenAsync(user) });
                    }
                }
                return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest("Server error");
            }
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var img = ImageWorker.FromBase64StringToImage(model.Photo);
            string randomFilename = Path.GetRandomFileName() + ".jpg";
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFilename);
            img.Save(dir, ImageFormat.Jpeg);
            var user = _mapper.Map<AppUser>(model);
            user.Photo = randomFilename;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors });


            return Ok(new { token = await _jwtTokenService.CreateTokenAsync(user) });
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IActionResult> Users()
        {
            throw new AppException("Email or password is incorrect");
            var list = _context.Users.Select(x => _mapper.Map<UserItemViewModel>(x)).ToList();
            Thread.Sleep(2000);
            return Ok(list);
        }

        [HttpGet]
        [Route("get-user/${id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserItemViewModel>(user));
        }




    }
}
