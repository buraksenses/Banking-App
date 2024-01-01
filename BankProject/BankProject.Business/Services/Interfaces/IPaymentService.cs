using BankProject.Business.DTOs.Payment;

namespace BankProject.Business.Services.Interfaces;

public interface IPaymentService
{
    Task<GetPaymentRequestDto> GetPaymentByIdAsync(Guid id);

    Task<List<GetPaymentRequestDto>> GetAllPaymentsAsync();

    Task<CreatePaymentRequestDto> CreatePaymentAsync(CreatePaymentRequestDto requestDto);

    Task<UpdatePaymentRequestDto> UpdatePaymentByIdAsync(Guid id, UpdatePaymentRequestDto requestDto);

    Task DeletePaymentByIdAsync(Guid id);
}