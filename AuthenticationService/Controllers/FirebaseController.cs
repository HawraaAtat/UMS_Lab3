using AuthenticationService.Domain.Models;
using AuthenticationService.DTO;
using AuthenticationService.Persistence;
using AutoMapper;
using Firebase.Auth;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using User = AuthenticationService.Domain.Models.User;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly FirebaseAuthProvider _auth;
        private readonly authContext _authContext;
        private readonly IMapper _mapper;
        //private readonly IConfiguration _configuration;
        private readonly IBus _bus;


        private const string API_KEY = "AIzaSyBNvRpjWrLo5gGU5wj3yXDPipmc7mu18RU";
        public FirebaseController(IMediator mediator, authContext authContext, IMapper mapper, IBus bus)
        {
            _mediator = mediator;
            _auth = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));
            _authContext = authContext;
            _mapper = mapper;
            //_configuration = configuration;
            _bus = bus; 

        }



        //[HttpPost("signup")]
        //[AllowAnonymous]
        //public async Task<ActionResult> SignUp(string email, string password)
        //{
        //    try
        //    {
        //        //create the user
        //        await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //        //log in the new user
        //        var fbAuthLink = await _auth.SignInWithEmailAndPasswordAsync(email, password);
        //        string token = fbAuthLink.FirebaseToken;
        //        return Ok("bearer " + token);
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("roles")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Role>> AddRole(string roleName)
        {
            try
            {
                var role = new Role { Name = roleName };
                _authContext.Roles.Add(role);
                await _authContext.SaveChangesAsync();
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(UserDTO userDTO)
        {
            try
            {
                //create the user
                await _auth.CreateUserWithEmailAndPasswordAsync(userDTO.Email, userDTO.Password);


                var user = new User { Name = userDTO.Name, Email = userDTO.Email, KeycloakId = Guid.NewGuid().ToString() };

                var role = await _authContext.Roles.FirstOrDefaultAsync(r => r.Name == userDTO.RoleName);
                if (role == null)
                {
                    return BadRequest($"Role {userDTO.RoleName} not found.");
                }
                user.Role = role;
                _authContext.Users.Add(user);
                await _authContext.SaveChangesAsync();

                //var user = _mapper.Map<User>(userDTO);

                var userDto = new UserDTO
                {
                    Name= userDTO.Name,
                    Email= userDTO.Email,
                    Password= userDTO.Password,
                    RoleName= userDTO.RoleName
                };

                try
                {
                    Uri uri = new Uri("rabbitmq://localhost/user");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    var serializedUser = JsonConvert.SerializeObject(userDto);

                    await endPoint.Send(userDto);

                }
                catch (Exception ex) 
                {
                    return BadRequest(ex);
                }


                //try
                //{
                //    var factory = new ConnectionFactory() { HostName = "localhost" };
                //    using (var connection = factory.CreateConnection())
                //    using (var channel = connection.CreateModel())
                //    {
                //        channel.QueueDeclare(queue: "user",
                //                             durable: false,
                //                             exclusive: false,
                //                             autoDelete: false,
                //                             arguments: null);

                //        var serializedUser = JsonConvert.SerializeObject(user);
                //        var body = Encoding.UTF8.GetBytes(serializedUser);

                //        channel.BasicPublish(exchange: "",
                //                             routingKey: "user",
                //                             basicProperties: null,
                //                             body: body);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    return BadRequest(ex);
                //}


                //log in the new user
                var fbAuthLink = await _auth.SignInWithEmailAndPasswordAsync(userDTO.Email, userDTO.Password);
                string token = fbAuthLink.FirebaseToken;
                return Ok("bearer " + token);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpPost("signin")]
        //[AllowAnonymous]
        //public async Task<ActionResult> SignIn(string email, string password)
        //{
        //    try
        //    {
        //        //log in an existing user
        //        var fbAuthLink = await _auth
        //            .SignInWithEmailAndPasswordAsync(email, password);
        //        string token = fbAuthLink.FirebaseToken;
        //        return Ok("bearer " + token);
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}



        //[HttpPost("signin")]
        //[AllowAnonymous]
        //public async Task<ActionResult> SignIn(string email, string password)
        //{
        //    try
        //    {
        //        //log in an existing user
        //        var fbAuthLink = await _auth
        //            .SignInWithEmailAndPasswordAsync(email, password);
        //        string token = fbAuthLink.FirebaseToken;

        //        // get the user's role from the database
        //        var user = await _authContext.Users
        //            .Include(u => u.Role)
        //            .FirstOrDefaultAsync(u => u.Email == email);
        //        if (user == null)
        //        {
        //            return BadRequest("User not found.");
        //        }

        //        // save the role in the JWT claims
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.Name),
        //            new Claim(ClaimTypes.Email, user.Email),
        //            new Claim(ClaimTypes.Role, user.Role.Name)
        //        };
        //        //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C1CF4B7DC4C4175B6618DE4F55CA4"));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(claims),
        //            Expires = DateTime.UtcNow.AddMinutes(30),
        //            SigningCredentials = creds
        //        };
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var jwtToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        //        var httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHandler.WriteToken(jwtToken));

        //        return Ok(new
        //        {
        //            token = "Bearer " + tokenHandler.WriteToken(jwtToken),
        //            expiration = jwtToken.ValidTo
        //        });
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(string email, string password)
        {
            try
            {
                // log in an existing user
                var fbAuthLink = await _auth
                    .SignInWithEmailAndPasswordAsync(email, password);
                string token = fbAuthLink.FirebaseToken;

                // get the user's role from the database
                var user = await _authContext.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                // generate a JWT token with the user's role
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.KeycloakId),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C1CF4B7DC4C4175B6618DE4F55CA4"));
                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = creds,
                    Audience = "My Audience",
                    Issuer = "MyAPI"
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }



}
