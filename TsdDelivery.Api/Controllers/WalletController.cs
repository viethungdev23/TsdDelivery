using Microsoft.AspNetCore.Mvc;
using TsdDelivery.Application.Interface;

namespace TsdDelivery.Api.Controllers;

public class WalletController : BaseController
{
    private readonly IWalletService _walletService;
    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        var response = await _walletService.GetWalletById(id);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpGet]
    public async Task<IActionResult> GetWalletByUserId(Guid userId)
    {
        var response = await _walletService.GetWalletByUserId(userId);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }
}