using AutoMapper;
using BankProject.Business.DTOs.Role;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Business.Services.Concretes;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        var role = await GetRoleOrThrow(id);

        var roleDto = _mapper.Map<RoleDto>(role);

        return roleDto;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var role = await GetRoleOrThrow(id);

        await _repository.DeleteAsync(role);
    }

    private async Task<Role> GetRoleOrThrow(Guid id)
    {
        var role = await _repository.GetByIdAsync(id);

        if (role == null)
            throw new NotFoundException("Role not found!");

        return role;
    }
}