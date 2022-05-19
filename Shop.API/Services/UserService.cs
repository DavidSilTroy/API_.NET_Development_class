using Shop.DAL.Data;
using Shop.API.Helpers;
using Shop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shop.API.Services;

namespace Shop.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ShopContext _shopContext;

        public UserService(IOptions<AppSettings> appSettings, ShopContext shopContext)
        {
            _appSettings = appSettings.Value;
            _shopContext = shopContext;
        }

        public User Authenticate(string username, string password)
        {
            var user = _shopContext.Users.Include(u => u.Customer).SingleOrDefault(x => x.UserName == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.UserId.ToString()),
                    new Claim("Email", user.Customer.Email),
                    new Claim("Username", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }
    }
}
