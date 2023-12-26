using BankProject.API.DTOs.User;

namespace BankProject.Business.Services.Interfaces;

public interface IUserService
{
    public Task<GetUserRequestDto> GetByIdAsync(Guid id);

    public Task CreateAsync(CreateUserRequestDto requestDto);

    public Task UpdateAsync(Guid id, UpdateUserRequestDto requestDto);

    public Task DeleteByIdAsync(Guid id);
}