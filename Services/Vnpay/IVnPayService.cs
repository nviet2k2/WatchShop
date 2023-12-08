
using Core.Domains;
using Microsoft.AspNetCore.Http;
using Services.DTOs;

public interface IVnPayService
{
    Task<string> CreatePaymentUrl(CreateOrderDTO model, HttpContext context,int UserId);
   
}