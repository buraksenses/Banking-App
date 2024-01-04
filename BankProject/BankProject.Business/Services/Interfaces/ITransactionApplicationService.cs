namespace BankProject.Business.Services.Interfaces;

public interface ITransactionApplicationService
{
    Task ApproveApplicationAsync(Guid id);

    Task RejectApplicationAsync(Guid id);
}