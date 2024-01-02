using BankProject.Business.DTOs.SupportTicket;
using BankProject.Core.Enums;

namespace BankProject.Business.Services.Interfaces;

public interface ISupportTicketService
{
    Task<GetSupportTicketRequestDto> GetByIdAsync(Guid id);

    Task<List<GetSupportTicketRequestDto>> GetAllAsync();

    Task<List<GetSupportTicketRequestDto>> GetAllSupportTicketsOfUserAsync(string userId);

    Task<CreateSupportTicketRequestDto> CreateAsync(CreateSupportTicketRequestDto requestDto);

    Task<GetSupportTicketRequestDto> UpdateTicketStatusByIdAsync(Guid id, TicketStatus newStatus);

    Task<GetSupportTicketRequestDto> UpdateTicketPriorityByIdAsync(Guid id, TicketPriority newPriority);
}