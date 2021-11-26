using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using T2008M_NetCoreApi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
namespace T2008M_NetCoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly T2008MContext _context;
        public IConfiguration _configuration;

        public LoginController(IConfiguration config,T2008MContext context)
        {
            _configuration = config;
            _context = context;
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] frData = Encoding.UTF8.GetBytes(str);
            byte[] toData = md5.ComputeHash(frData);
            string hashString = "";
            for (int i = 0; i < toData.Length; i++)
            {
                hashString += toData[i].ToString("x2");
            }
            return hashString;
        }


        [HttpPost]
        public async Task<IActionResult> Login(User _userData)
        {
            if(_userData != null && _userData.Email !=null && _userData.Password != null)
            {
               // _userData.Password = GetMD5(_userData.Password);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userData.Email && u.Password == _userData.Password);
                if(user != null)
                {
                    // tao 1 token de tra ve cho client
                    var claims = new[] { 
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToString()),
                        new Claim("Id",user.Id.ToString()),
                        new Claim("FullName",user.FullName),
                        new Claim("Email",user.Email),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                        expires:DateTime.Now.AddDays(1),signingCredentials:sign);
                    // tra token ve client
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                return BadRequest("Email or Password invalid!");
            }
            return BadRequest();
        }
    }
}
