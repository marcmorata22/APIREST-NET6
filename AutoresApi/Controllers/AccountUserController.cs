using AutoresApi.DTOs;
using AutoresApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoresApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountUserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly HashService _hashService;
        private readonly IDataProtector dataProtector;

        public AccountUserController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            IDataProtectionProvider dataProtectionProvider,
            HashService hashService)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._signInManager = signInManager;
            this._hashService = hashService;
            dataProtector = dataProtectionProvider.CreateProtector("secretValue");
        }

        [HttpGet("encrypt")]
        public ActionResult EncryptTime()
        {
            var protectorTime = dataProtector.ToTimeLimitedDataProtector();
            var planetext = "Marc Morata";
            var encryptText = protectorTime.Protect(planetext, lifetime: TimeSpan.FromSeconds(5));
            //Time for lose Encrypt
            //Thread.Sleep(6000);
            var unencrypted = protectorTime.Unprotect(encryptText);

            return Ok(new
            {
                planetext = planetext,
                encryptText = encryptText,
                unencrypted =  unencrypted
            });
        }

        [HttpGet("hash/{planeText}")]
        public ActionResult PerformHash(string planeText)
        {
            var result1 = _hashService.Hash(planeText);
            var result2 = _hashService.Hash(planeText);
            return Ok(new
            {
                planeText = planeText,
                Hash1 = result1,
                Hash2 = result2
            });
        }


        [HttpPost("register")]
        public async Task<ActionResult<AnswerAutenticationDTO>> RegisterUser(CredentialUsersDTO credentials)
        {
            var user = new IdentityUser
            {
                UserName = credentials.Email,
                Email = credentials.Email
            };

            var result = await _userManager.CreateAsync(user, credentials.Password);

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AnswerAutenticationDTO>> Login(CredentialUsersDTO credentials)
        {
            var result = await _signInManager.PasswordSignInAsync(credentials.Email,
                credentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest("Error login");
            }
        }

        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AnswerAutenticationDTO>> Renew()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credential = new CredentialUsersDTO()
            {
                Email = email
            };

            return await BuildToken(credential);
        }


        private async Task<AnswerAutenticationDTO> BuildToken(CredentialUsersDTO credentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credentials.Email)
            };

            var user = await _userManager.FindByEmailAsync(credentials.Email);
            var ClaimsDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(ClaimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyJWT"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires:expiry, signingCredentials: creds);

            return new AnswerAutenticationDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                ExpiryDated = expiry
            };
        }

        [HttpPost("MakeAdmin")]
        public async Task<ActionResult> MakeAdmin(EditAdminDTO editAdminDTO)
        {
            var user = await _userManager.FindByEmailAsync(editAdminDTO.Email);
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoveAdmin")]
        public async Task<ActionResult> RemoveAdmin(EditAdminDTO editAdminDTO)
        {
            var user = await _userManager.FindByEmailAsync(editAdminDTO.Email);
            await _userManager.RemoveClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }
    }
}
