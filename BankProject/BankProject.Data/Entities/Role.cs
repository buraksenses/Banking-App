using BankProject.Core.Enums;

namespace BankProject.Data.Entities;

public class Role
{
    public Guid Id { get; set; }

    public RoleType Name { get; set; }
    
    //Navigation Properties
    public ICollection<User> Users { get; set; }
}