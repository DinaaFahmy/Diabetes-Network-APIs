using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebApplication.Configuration;
using WebApplication.Hubs;
using WebApplication.Models;
using WebApplication.ModelViews;
using WebApplication.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public ImessageHub messageHub;

        private readonly DiabetesSystem2Context _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _Config;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        public WebApplication.Repo.IDiabetes diabetes ;
        public AccountController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, DiabetesSystem2Context db, UserManager<ApplicationUser> manager,WebApplication.Repo.IDiabetes _diabetes, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, ImessageHub m)
        {
            _db = db;
            _manager = manager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _Config = configuration;
            jwtBearerTokenSettings = jwtTokenOptions.Value;
            messageHub = m;
            diabetes = _diabetes;
        }

        [HttpPost]
        [Route("RegisterAspatient")]
        public async Task<IActionResult> RegisterAspatient(RegisterAsPatientModel model)
        {
            await CreateRoles();
            if (model == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (EmailExist(model.Email))
                {
                    return BadRequest("Email is Exist");
                }

                if (!ValidEmail(model.Email))
                {
                    return BadRequest("Email is Not valid");
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0]
                };

                var result = await _manager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    Users u = new Users();
                    u.ID = user.Id;
                    u.UserName = model.UserName;
                    u.Type = false;

                    _db.users.Add(u);
                    _db.SaveChanges();

                    Patient p = new Patient();
                    p.PatientId = u.UserId;
                    p.BirthDate = model.BirthDate;
                    p.Gender = model.Gender;
                    p.Height = model.Height;
                    p.Weight = model.Weight;
                    p.LifeStyle = model.LifeStyle;
                    p.MedicalCondetion = model.MedicalCondetion;


                    _db.Patient.Add(p);
                    _db.SaveChanges();
                    if (await _roleManager.RoleExistsAsync("Patient"))
                    { await _manager.AddToRoleAsync(user, "Patient"); }


                    var tocken = await _manager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmlink = Url.Action("Registerconfirm", "Account", new { ID = user.Id, Tocken = HttpUtility.UrlEncode(tocken) }, Request.Scheme);
                    var link = "<a href=\"" + confirmlink + "\">Confirm Registration</a>";
                    if (await SendGridApi.Execute(user.Email, user.UserName, "Please Confirm your Email", link, "Registration to Diabetes Site Confirm"))
                    {
                        return Ok(confirmlink);
                    }


                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }


        [HttpPost]
        [Route("RegisterAsDoctor")]
        public async Task<IActionResult> RegisterAsDoctor(RegisterAsDoctorModel model)
        {
            await CreateRoles();
            if (model == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (EmailExist(model.Email))
                {
                    return BadRequest("Email is Exist");
                }

                if (!ValidEmail(model.Email))
                {
                    return BadRequest("Email is Not valid");
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],
                    PhoneNumber = model.PhoneNumber
                };


                var result = await _manager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    Users u = new Users();
                    u.ID = user.Id;
                    u.UserName = model.UserName;
                    u.Type = true;

                    _db.users.Add(u);
                    _db.SaveChanges();

                    Doctor d = new Doctor();
                    d.DoctorId = u.UserId;
                    d.Address = model.Address;
                    _db.Doctor.Add(d);
                    _db.SaveChanges();

                    if (await _roleManager.RoleExistsAsync("Doctor"))
                    { await _manager.AddToRoleAsync(user, "Doctor"); }


                    var tocken = await _manager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmlink = Url.Action("Registerconfirm", "Account", new { ID = user.Id, Tocken = HttpUtility.UrlEncode(tocken) }, Request.Scheme);
                    var link = "<a href=\"" + confirmlink + "\">Confirm Registration</a>";

                    if (await SendGridApi.Execute(user.Email, user.UserName, "Please Confirm your Email", link, "Registration to Diabetes Site Confirm"))
                    {
                        return Ok(confirmlink);
                    }


                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest);


        }

        [HttpPost]
        [Route("EmailExist")]
        public bool EmailExist([FromBody] string email)
        {
            return _db.Users.Any(a => a.Email == email);
        }

        [HttpPost]
        [Route("Emailconfirmed")]
        public async Task<bool> Emailconfirmed([FromBody] string email)
        {
            var user = await _manager.FindByEmailAsync(email);

            if (user == null)
            {
                return true;
            }

            return user.EmailConfirmed;

        }

        public static bool ValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        [HttpGet]
        [Route("Registerconfirm")]
        public async Task<IActionResult> Registerconfirm(string ID, string Tocken)
        {
            if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Tocken))
            {
                return NotFound();
            }
            var user = await _manager.FindByIdAsync(ID);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _manager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(Tocken));

            if (result.Succeeded)
            {
                return Ok("Register success");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        //Login With Cookie
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (model == null)
            {
                return NotFound();
            }
            var user = await _manager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return NotFound();
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized("Email not confirmed yet");
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var role = await GerRoleNameById(user.Id);
                if (role != null)
                {
                    AddCookie(user.UserName, role, user.Id, model.RememberMe);
                }
                return Ok("Login success");
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized("user account is locked");
            }

            return StatusCode(StatusCodes.Status204NoContent);


        }

        private async Task<string> GerRoleNameById(string id)
        {
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(a => a.UserId == id);
            if (userRole != null)
            {
                return await _db.Roles.Where(a => a.Id == userRole.RoleId).Select(b => b.Name).FirstOrDefaultAsync();
            }
            return null;
        }

        private async Task CreateRoles()
        {
            if (_roleManager.Roles.Count() < 1)
            {
                var role = new ApplicationRole() { Name = "Doctor" };
                await _roleManager.CreateAsync(role);

                role = new ApplicationRole() { Name = "Patient" };
                await _roleManager.CreateAsync(role);
            }


        }

        public async void AddCookie(string username, string rolename, string userid, bool remember)
        {
            var claim = new List<Claim>
            {
            new Claim(ClaimTypes.Name,username),
           new Claim(ClaimTypes.NameIdentifier, userid),
           new Claim(ClaimTypes.Role,rolename),

            };

            var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

            if (remember)
            {
                var authProperties = new AuthenticationProperties { AllowRefresh = true, IsPersistent = remember, ExpiresUtc = DateTime.Now.AddDays(10) };
                await HttpContext.SignInAsync
                    (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperties

                   );

            }
            else
            {
                var authProperties = new AuthenticationProperties { AllowRefresh = true, IsPersistent = remember, ExpiresUtc = DateTime.Now.AddMinutes(30) };
                await HttpContext.SignInAsync
                    (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity),
                    authProperties

                   );
            }


        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }


        //----------------with token-------------------------------------------

        [HttpPost]
        [Route("Loginn")]
        public async Task<IActionResult> Loginn(LoginModel model)
        {

            var user = await _manager.FindByEmailAsync(model.Email);

          


            if (model == null)
            {
                return NotFound();
            }
             //SendGridApi.Execute().Wait();



            if (user == null)
            {
                return NotFound();
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized("Email not confirmed yet");
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var identityUser = await _manager.FindByEmailAsync(model.Email);
                var role = await GerRoleNameById(user.Id);
                var tokenstr = GenerateToken(identityUser, role,model.RememberMe);
                return Ok(new { token = tokenstr });
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized("user account is locked");
            }

            return    BadRequest("كلمة المرور غير صحيحة");

        }

        private object GenerateToken(IdentityUser identityUser, string role,bool rememberme)
        {
            double Expire;
            if(rememberme)
            {
                Expire = 3600;
            }
            else
            {
                Expire = jwtBearerTokenSettings.ExpiryTimeInSeconds;
            }

            var userName = _db.users.Where(a=>a.ID==identityUser.Id).FirstOrDefault().UserName;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                        new Claim(ClaimTypes.Email, identityUser.Email),
                        new Claim(ClaimTypes.NameIdentifier,identityUser.Id),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                        new Claim(ClaimTypes.GivenName,userName),
                        new Claim(ClaimTypes.Surname,diabetes.GetUserID(identityUser.Id).ToString()),

                }),

                Expires = DateTime.UtcNow.AddSeconds(Expire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };
            //messageHub.AddUser(diabetes.GetUserID(identityUser.Id));
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }


    }
}