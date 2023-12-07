using Core.Domains;
using Microsoft.AspNetCore.Http;
using Services.DTOs;

namespace Services.Paypal;
public interface IPayPalService
{
    Task<string> CreatePaymentUrl(CreateOrderDTO model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}