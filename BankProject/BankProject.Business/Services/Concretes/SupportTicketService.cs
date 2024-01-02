using AutoMapper;
using BankProject.Business.DTOs.SupportTicket;
using BankProject.Business.Helpers;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Business.Services.Concretes;

public class SupportTicketService : ISupportTicketService
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IMapper _mapper;

    public SupportTicketService(ISupportTicketRepository supportTicketRepository,
        IMapper mapper)
    {
        _supportTicketRepository = supportTicketRepository;
        _mapper = mapper;
    }
    
    public async Task<GetSupportTicketRequestDto> GetByIdAsync(Guid id)
    {
        var ticket = await _supportTicketRepository.GetOrThrowAsync(id);

        var ticketDto = _mapper.Map<GetSupportTicketRequestDto>(ticket);

        return ticketDto;
    }

    public async Task<List<GetSupportTicketRequestDto>> GetAllAsync()
    {
        var ticketList = await _supportTicketRepository.GetAllAsync();

        var ticketListDto = _mapper.Map<List<GetSupportTicketRequestDto>>(ticketList);

        return ticketListDto;
    }

    public async Task<List<GetSupportTicketRequestDto>> GetAllSupportTicketsOfUserAsync(string userId)
    {
        var ticketList = await _supportTicketRepository.GetAllAsync(ticket => ticket.UserId == userId);

        var ticketListDto = _mapper.Map<List<GetSupportTicketRequestDto>>(ticketList);

        return ticketListDto;
    }

    public async Task<CreateSupportTicketRequestDto> CreateAsync(CreateSupportTicketRequestDto requestDto)
    {
        var ticket = _mapper.Map<SupportTicket>(requestDto);

        await _supportTicketRepository.CreateAsync(ticket);

        return requestDto;
    }

    public async Task<GetSupportTicketRequestDto> UpdateTicketStatusByIdAsync(Guid id, TicketStatus newStatus)
    {
        var ticket = await _supportTicketRepository.GetOrThrowAsync(id);

        ticket.TicketStatus = newStatus;

        await _supportTicketRepository.UpdateAsync(id, ticket);

        var ticketDto = _mapper.Map<GetSupportTicketRequestDto>(ticket);

        return ticketDto;
    }

    public async Task<GetSupportTicketRequestDto> UpdateTicketPriorityByIdAsync(Guid id, TicketPriority newPriority)
    {
        var ticket = await _supportTicketRepository.GetOrThrowAsync(id);

        ticket.TicketPriority = newPriority;
        
        await _supportTicketRepository.UpdateAsync(id, ticket);

        var ticketDto = _mapper.Map<GetSupportTicketRequestDto>(ticket);

        return ticketDto;
    }
}