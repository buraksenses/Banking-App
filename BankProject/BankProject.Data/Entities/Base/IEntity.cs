namespace BankProject.Data.Entities.Base;

public interface IEntity<T>
{
    public T Id { get; set; }
}