using BankProject.Data.Entities;

namespace BankProject.Business.Services.Interfaces;

public interface ICreditScoreService
{
    float CalculateCreditScore(User user);
}