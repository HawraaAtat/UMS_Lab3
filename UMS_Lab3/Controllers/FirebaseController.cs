using AutoMapper;
using Firebase.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UMS_Lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly FirebaseAuthProvider _auth;
        private const string API_KEY = "AIzaSyBNvRpjWrLo5gGU5wj3yXDPipmc7mu18RU";
        public FirebaseController(IMediator mediator)
        {
            _mediator = mediator;
            _auth = new FirebaseAuthProvider(new FirebaseConfig(API_KEY));
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(string email, string password)
        {
            try
            {
                //log in an existing user
                var fbAuthLink = await _auth
                    .SignInWithEmailAndPasswordAsync(email, password);
                string token = fbAuthLink.FirebaseToken;
                return Ok("bearer " + token);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(string email, string password)
        {
            try
            {
                //create the user
                await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                //log in the new user
                var fbAuthLink = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                string token = fbAuthLink.FirebaseToken;
                return Ok("bearer " + token);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }



}
