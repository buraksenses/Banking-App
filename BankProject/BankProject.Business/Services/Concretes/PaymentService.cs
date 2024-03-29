﻿using AutoMapper;
using BankProject.Business.DTOs.Payment;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;
using Hangfire;

namespace BankProject.Business.Services.Concretes;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountService _accountService;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(
        IMapper mapper,
        IAccountService accountService,
        IUnitOfWork unitOfWork, 
        IPaymentRepository paymentRepository, 
        IAccountRepository accountRepository)
    {
        _mapper = mapper;
        _accountService = accountService;
        _unitOfWork = unitOfWork;
        _paymentRepository = paymentRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<GetPaymentRequestDto> GetPaymentByIdAsync(Guid id)
    {
        var payment = await _paymentRepository.GetOrThrowNotFoundByIdAsync(id);

        var paymentDto = _mapper.Map<GetPaymentRequestDto>(payment);

        return paymentDto;
    }

    public async Task<List<GetPaymentRequestDto>> GetAllPaymentsAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();

        var paymentListDto = _mapper.Map<List<GetPaymentRequestDto>>(payments);

        return paymentListDto;
    }

    public async Task<CreatePaymentRequestDto> CreatePaymentAsync(CreatePaymentRequestDto requestDto)
    {
        if (!Enum.TryParse(requestDto.TimePeriod, true, out TimePeriod timePeriod))
        {
            throw new ArgumentException("Invalid time period.");
        }

        requestDto.TimePeriod = timePeriod.ToString();

        var payment = _mapper.Map<Payment>(requestDto);

        var account = await _accountRepository.GetOrThrowNotFoundByIdAsync(payment.AccountId);
        
        ValidateAccountBalance(account,requestDto.Amount);

        var cronExpression = GenerateCronExpression(payment.TimePeriod, payment.PaymentFrequency);
        
        await _paymentRepository.CreateAsync(payment);

        RecurringJob.AddOrUpdate<PaymentService>(
            $"payment-job-{payment.Id}",
            service => service.MakePayment(payment,payment.Amount),
            cronExpression);

        await _unitOfWork.SaveChangesAsync();
        
        return requestDto;
    }

    public async Task<UpdatePaymentRequestDto> UpdatePaymentByIdAsync(Guid id, UpdatePaymentRequestDto requestDto)
    {
        if (!Enum.TryParse(requestDto.TimePeriod, true, out TimePeriod timePeriod))
        {
            throw new ArgumentException("Invalid time period.");
        }

        requestDto.TimePeriod = timePeriod.ToString();

        var existingPayment = await _paymentRepository.GetOrThrowNotFoundByIdAsync(id);
        
        _mapper.Map(requestDto, existingPayment);
    
        var account = await _accountRepository.GetOrThrowNotFoundByIdAsync(existingPayment.AccountId);
        
        ValidateAccountBalance(account,requestDto.Amount);

        var cronExpression = GenerateCronExpression(existingPayment.TimePeriod, existingPayment.PaymentFrequency);

        RecurringJob.AddOrUpdate<PaymentService>(
            $"payment-job-{existingPayment.Id}",
            service => service.MakePayment(existingPayment, existingPayment.Amount),
            cronExpression);

        await _paymentRepository.UpdateAsync(id, existingPayment);

        await _unitOfWork.SaveChangesAsync();

        return requestDto;
    }

    public async Task DeletePaymentByIdAsync(Guid id)
    {
        await _paymentRepository.DeleteAsync(id);
        
        RecurringJob.RemoveIfExists($"payment-job-{id}");

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MakePayment(Payment payment, decimal amount)
    {
        payment.LastPaymentDate = DateTime.UtcNow;
        payment.NextPaymentDate =
            CalculateNextPaymentDate(payment.LastPaymentDate, payment.TimePeriod, payment.PaymentFrequency);

        await _accountService.MakeBillPayment(payment.AccountId, amount);
        await _paymentRepository.UpdateAsync(payment.Id, payment);
        await _unitOfWork.SaveChangesAsync();
    }
    
    private static DateTime CalculateNextPaymentDate(DateTime lastPaymentDate, TimePeriod timePeriod, int paymentFrequency)
    {
        return timePeriod switch
        {
            TimePeriod.Minutes => lastPaymentDate.AddMinutes(paymentFrequency),
            TimePeriod.Hours => lastPaymentDate.AddHours(paymentFrequency),
            TimePeriod.Days => lastPaymentDate.AddDays(paymentFrequency),
            TimePeriod.Months => lastPaymentDate.AddMonths(paymentFrequency),
            _ => throw new ArgumentException("Invalid time period")
        };
    }

    private static string GenerateCronExpression(TimePeriod timePeriod, int frequency)
    {
        return timePeriod switch
        {
            TimePeriod.Minutes => $"*/{frequency} * * * *",
            TimePeriod.Hours => $"0 */{frequency} * * *",
            TimePeriod.Days => $"0 0 */{frequency} * *",
            TimePeriod.Months => $"0 0 1 */{frequency} *",
            _ => throw new ArgumentException("Invalid time period")
        };
    }

    private static void ValidateAccountBalance(Account account, decimal amount)
    {
        if (account.Balance < amount)
            throw new InsufficientFundsException("Insufficient funds!");
    }
}