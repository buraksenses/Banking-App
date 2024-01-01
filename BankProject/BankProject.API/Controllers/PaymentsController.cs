using BankProject.Business.DTOs.Payment;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

public class PaymentsController : CustomControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [Route("create-payment")]
    public async Task<IActionResult> CreateAsync(CreatePaymentRequestDto requestDto)
    {
        await _paymentService.CreatePaymentAsync(requestDto);

        return Ok(requestDto);
    }

    [HttpPut]
    [Route("{id:guid}/update")]
    public async Task<IActionResult> UpdateByIdAsync(Guid id, UpdatePaymentRequestDto requestDto)
    {
        await _paymentService.UpdatePaymentByIdAsync(id, requestDto);

        return Ok(requestDto);
    }

    [HttpDelete]
    [Route("{id:guid}/delete")]
    public async Task<IActionResult> DeleteByIdAsync(Guid id)
    {
        await _paymentService.DeletePaymentByIdAsync(id);

        return Ok(id);
    }
}