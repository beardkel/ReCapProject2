﻿using Business.Abstract;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var result = _authService.Register(userForRegisterDto);
            if (!result.Success) return BadRequest(result);

            var createAccessTokenResult = _authService.CreateAccessToken(result.Data);
            if (!result.Success) return BadRequest(result);

            var newSuccessDataResult = new SuccessDataResult<AccessToken>(createAccessTokenResult.Data, result.Message);
            return Ok(newSuccessDataResult);
            //var userExists = _authService.UserExists(userForRegisterDto.Email);
            //if (!userExists.Success)
            //{
            //    return BadRequest(userExists.Message);
            //}

            //var registerResult = _authService.Register(userForRegisterDto);
            //var result = _authService.CreateAccessToken(registerResult.Data);
            //if (result.Success)
            //{
            //    return Ok(result.Data);
            //}

            //return BadRequest(result.Message);
        }
    }
}
