﻿using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface ILoanRepository : IGenericRepository<Loan,Guid>
{
}