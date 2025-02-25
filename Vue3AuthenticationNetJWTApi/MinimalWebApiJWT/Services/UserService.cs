using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MinimalWebApiJWT.Helpers;
using MinimalWebApiJWT.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalWebApiJWT.Services
{

    public interface IUserService
    {
        User FindByUsername(string username);

        IEnumerable<User> GetAll();

        string CreateToken(User user);
    }

    public class UserService : IUserService
    {

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" },
            new User { Id = 2, FirstName = "John", LastName = "Doe", Username = "john", Password = "doe" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User FindByUsername(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public string CreateToken(User user)
        {
            // generate token that is valid for 1 hour
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
