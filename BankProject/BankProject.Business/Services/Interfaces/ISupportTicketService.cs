using BankProject.Business.DTOs.SupportTicket;

namespace BankProject.Business.Services.Interfaces;

public interface ISupportTicketService
{
    Task<GetSupportTicketRequestDto> GetByIdAsync(Guid id);

    Task<List<GetSupportTicketRequestDto>> GetAllAsync();

    Task<List<GetSupportTicketRequestDto>> GetAllByUserIdAsync(string userId);

    Task<CreateSupportTicketRequestDto> CreateAsync(CreateSupportTicketRequestDto requestDto);

    Task<GetSupportTicketRequestDto> UpdateTicketStatusByIdAsync(Guid id, string newStatus);

    Task<GetSupportTicketRequestDto> UpdateTicketPriorityByIdAsync(Guid id, string newPriority);
}