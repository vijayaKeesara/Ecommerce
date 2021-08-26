using ECommerce.BL.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {      
        private IConfiguration _config;
       
        public AuthenticateController(IConfiguration config)
        {
            _config = config;
        }

        #region GenerateJWT
        /// <summary>
        /// Generate Json Web Token Method
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJSONWebToken(LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.Name, userInfo.UserName)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              null,
              expires: DateTime.Now.AddHours(3),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);           
        }
        #endregion

        #region AuthenticateUser
        /// <summary>
        /// Hardcoded the User authentication
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private async Task<LoginModel> AuthenticateUser(LoginModel login)
        {
            LoginModel user = null;

            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.UserName == "test")
            {
                user = new LoginModel { UserName = "test", Password = "123456" };
            }
            return user;
        }
        #endregion

        #region Login Validation
        /// <summary>
        /// Login Authenticaton using JWT Token Authentication
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(data);
            if (data != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { Token = tokenString, Message = "Success" });
            }
            return response;
        }

        #endregion

        #region Get
       
        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<string>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }


        #endregion

    }  

}
