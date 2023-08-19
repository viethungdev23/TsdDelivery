﻿using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using TsdDelivery.Application.Interface;
using TsdDelivery.Application.Models.User.Request;
using TsdDelivery.Application.Models.User.Response;
using TsdDelivery.Application.Services;

namespace TsdDelivery.Api.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _userService.GetAllUsers();

        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }
        return Ok(response.Payload);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserCreateUpdate request)
    {
        var response = await _userService.Register(request);

        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }
        return Ok("SUCCESS");
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    { 
        var response = await _userService.Login(request);

        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }
        return Ok(response.Payload);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var response = await _userService.DeleteUser(id);
        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }
        return Ok("Delete Success");
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UploadAvatar(Guid id, IFormFile blob)
    {
        var response = await _userService.UploadImage(id, blob);
        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }
        return Ok(response.Payload);
    }
}
