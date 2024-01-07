using BankProject.Business.DTOs.SupportTicket;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

public class SupportTicketsController : CustomControllerBase
{
    private readonly ISupportTicketService _supportTicketService;

    public SupportTicketsController(ISupportTicketService supportTicketService)
    {
        _supportTicketService = supportTicketService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSupportTicketRequestDto requestDto)
    {
        await _supportTicketService.CreateAsync(requestDto);

        return Ok(requestDto);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var ticket = await _supportTicketService.GetByIdAsync(id);

        return Ok(ticket);
    }

    [HttpGet]
    [Route("user/{userId}")]
    public async Task<IActionResult> GetAllByUserIdAsync(string userId)
    {
        var userTickets = await _supportTicketService.GetAllByUserIdAsync(userId);

        return Ok(userTickets);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Auditor")]
    public async Task<IActionResult> GetAllAsync()
    {
        var tickets = await _supportTicketService.GetAllAsync();

        return Ok(tickets);
    }

    [HttpPut]
    [Route("{id:guid}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTicketStatusAsync(Guid id, string ticketStatus)
    {
        var ticketDto = await _supportTicketService.UpdateTicketStatusByIdAsync(id, ticketStatus);

        return Ok(ticketDto);
    }
    
    [HttpPut]
    [Route("{id:guid}/priority")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTicketPriorityAsync(Guid id, string ticketPriority)
    {
        var ticketDto = await _supportTicketService.UpdateTicketPriorityByIdAsync(id, ticketPriority);

        return Ok(ticketDto);
    }
}