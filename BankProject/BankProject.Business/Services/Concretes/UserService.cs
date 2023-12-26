using AutoMapper;
using BankProject.API.DTOs.User;
using BankProject.Business.DTOs.Role;
using BankProject.Business.DTOs.User;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Business.Services.Concretes;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<GetUserRequestDto> GetByIdAsync(Guid id)
    {
        var user = await GetUserOrThrow(id);

        var userDto = _mapper.Map<GetUserRequestDto>(user);

        return userDto;
    }

    public async Task CreateAsync(CreateUserRequestDto requestDto)
    {
        var user = _mapper.Map<User>(requestDto);

        await _repository.CreateAsync(user);
    }

    public async Task UpdateAsync(Guid id, UpdateUserRequestDto requestDto)
    {
        var updatingUser = await GetUserOrThrow(id);

        var newUser = _mapper.Map<User>(requestDto);

        await _repository.UpdateAsync(updatingUser, newUser);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var deletingUser = await GetUserOrThrow(id);
        await _repository.DeleteAsync(deletingUser);
    }

    public async Task AssignRoleAsync(Guid id, CreateRoleRequestDto requestDto)
    {
        var user = await GetUserOrThrow(id);
        
        if (!Enum.TryParse(requestDto.Name, true, out RoleType roleType))
        {
            throw new ArgumentException("Invalid role type.");
        }
        
        requestDto.Name = roleType.ToString();

        var role = _mapper.Map<Role>(requestDto);

        await _repository.AssignRoleAsync(user, role);
    }

    public async Task<List<GetUserRequestDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();

        var requestDtos = _mapper.Map<List<GetUserRequestDto>>(users);

        return requestDtos;
    }

    private async Task<User> GetUserOrThrow(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found!");
        
        return user;
    }

}