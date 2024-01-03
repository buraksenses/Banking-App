using AutoMapper;
using BankProject.Business.DTOs.SupportTicket;
using BankProject.Business.Helpers;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Services.Concretes;

public class SupportTicketService : ISupportTicketService
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public SupportTicketService(
        IMapper mapper,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _supportTicketRepository = unitOfWork.GetRepository<SupportTicketRepository, SupportTicket, Guid>();
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
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

    public async Task<List<GetSupportTicketRequestDto>> GetAllByUserIdAsync(string userId)
    {
        var ticketList = await _supportTicketRepository.GetAllAsync(ticket => ticket.UserId == userId);

        var ticketListDto = _mapper.Map<List<GetSupportTicketRequestDto>>(ticketList);

        return ticketListDto;
    }

    public async Task<CreateSupportTicketRequestDto> CreateAsync(CreateSupportTicketRequestDto requestDto)
    {
        var user = await _userManager.FindByIdAsync(requestDto.UserId);

        if (user == null)
            throw new NotFoundException("User not found");
        
        var ticket = _mapper.Map<SupportTicket>(requestDto);

        await _supportTicketRepository.CreateAsync(ticket);

        await _unitOfWork.SaveChangesAsync();

        return requestDto;
    }

    public async Task<GetSupportTicketRequestDto> UpdateTicketStatusByIdAsync(Guid id, string newStatus)
    {
        if (!Enum.TryParse(newStatus, true, out TicketStatus ticketStatus))
        {
            throw new ArgumentException("Invalid account type.");
        }
        
        var ticket = await _supportTicketRepository.GetOrThrowAsync(id);

        ticket.TicketStatus = ticketStatus;

        await _supportTicketRepository.UpdateAsync(id, ticket);

        var ticketDto = _mapper.Map<GetSupportTicketRequestDto>(ticket);

        await _unitOfWork.SaveChangesAsync();

        return ticketDto;
    }

    public async Task<GetSupportTicketRequestDto> UpdateTicketPriorityByIdAsync(Guid id, string newPriority)
    {
        if (!Enum.TryParse(newPriority, true, out TicketPriority ticketPriority))
        {
            throw new ArgumentException("Invalid account type.");
        }
        
        var ticket = await _supportTicketRepository.GetOrThrowAsync(id);

        ticket.TicketPriority = ticketPriority;
        
        await _supportTicketRepository.UpdateAsync(id, ticket);

        var ticketDto = _mapper.Map<GetSupportTicketRequestDto>(ticket);

        await _unitOfWork.SaveChangesAsync();

        return ticketDto;
    }
}