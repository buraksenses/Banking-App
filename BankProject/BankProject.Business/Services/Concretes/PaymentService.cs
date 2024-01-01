﻿using AutoMapper;
using BankProject.Business.DTOs.Payment;
using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Core.Exceptions;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Hangfire;

namespace BankProject.Business.Services.Concretes;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;


    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IAccountRepository accountRepository,IAccountService accountService)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _accountService = accountService;
    }
    
    public async Task<GetPaymentRequestDto> GetPaymentByIdAsync(Guid id)
    {
        var payment = await GetPaymentOrThrow(id);

        var paymentDto = _mapper.Map<GetPaymentRequestDto>(payment);

        return paymentDto;
    }

    public async Task<List<GetPaymentRequestDto>> GetAllPaymentsAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();

        var paymentDtos = _mapper.Map<List<GetPaymentRequestDto>>(payments);

        return paymentDtos;
    }

    public async Task<CreatePaymentRequestDto> CreatePaymentAsync(CreatePaymentRequestDto requestDto)
    {
        if (!Enum.TryParse(requestDto.TimePeriod, true, out TimePeriod timePeriod))
        {
            throw new ArgumentException("Invalid time period.");
        }

        requestDto.TimePeriod = timePeriod.ToString();

        var payment = _mapper.Map<Payment>(requestDto);

        await _accountService.GetAccountOrThrow(account => account.Id == payment.AccountId && account.Balance > payment.Amount);

        var cronExpression = GenerateCronExpression(payment.TimePeriod, payment.PaymentFrequency);
        
        await _paymentRepository.CreateAsync(payment);

        RecurringJob.AddOrUpdate<PaymentService>(
            $"payment-job-{payment.Id}",
            service => service.MakePayment(payment,payment.Amount),
            cronExpression);

        
        return requestDto;
    }

    public async Task<UpdatePaymentRequestDto> UpdatePaymentByIdAsync(Guid id, UpdatePaymentRequestDto requestDto)
    {
        if (!Enum.TryParse(requestDto.TimePeriod, true, out TimePeriod timePeriod))
        {
            throw new ArgumentException("Invalid time period.");
        }

        requestDto.TimePeriod = timePeriod.ToString();

        var existingPayment = await GetPaymentOrThrow(id);
    
        // 'existingPayment' nesnesinin özelliklerini 'requestDto' ile güncelle
        _mapper.Map(requestDto, existingPayment);
    
        await _accountService.GetAccountOrThrow(account => account.Id == existingPayment.AccountId && account.Balance > existingPayment.Amount);

        var cronExpression = GenerateCronExpression(existingPayment.TimePeriod, existingPayment.PaymentFrequency);

        RecurringJob.AddOrUpdate<PaymentService>(
            $"payment-job-{existingPayment.Id}",
            service => service.MakePayment(existingPayment, existingPayment.Amount),
            cronExpression);

        await _paymentRepository.UpdateAsync(id, existingPayment);

        return requestDto;
    }

    public async Task DeletePaymentByIdAsync(Guid id)
    {
        await _paymentRepository.DeleteAsync(id);
        
        RecurringJob.RemoveIfExists($"payment-job-{id}");
    }

    public async Task MakePayment(Payment payment, float amount)
    {
        payment.LastPaymentDate = DateTime.UtcNow;
        payment.NextPaymentDate =
            CalculateNextPaymentDate(payment.LastPaymentDate, payment.TimePeriod, payment.PaymentFrequency);
        await _accountService.MakePayment(payment.AccountId, amount);
        await _paymentRepository.UpdateAsync(payment.Id, payment);
    }
    
    private async Task<Payment> GetPaymentOrThrow(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        
        if (payment == null)
            throw new NotFoundException("Payment not found");
        
        return payment;
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
}