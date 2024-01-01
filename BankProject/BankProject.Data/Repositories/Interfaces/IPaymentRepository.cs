using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface IPaymentRepository : ICreateRepository<Payment,Guid>, IReadRepository<Payment,Guid>, IUpdateRepository<Payment,Guid>,IDeleteRepository<Payment,Guid>
{
    
}