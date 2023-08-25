﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TsdDelivery.Api.Filters;
using TsdDelivery.Application.Interface;
using TsdDelivery.Application.Models.Role.Request;

namespace TsdDelivery.Api.Controllers;


public class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var response = await _roleService.GetAllRoles();

        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateRole(RoleCreateUpdate request)
    {
        var response = await _roleService.CreateRole(request);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok("Success");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var response = await _roleService.DeleteRole(id);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok("Delete Success");
    }
}
