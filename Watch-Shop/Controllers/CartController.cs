using CodeMegaVNPay.Models;
using Core.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Services;
using Services.DTOs;
using System.Data;
using System.Security.Claims;

namespace Watch_Shop.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IVnPayService _vnPayService;
        public CartController(IOrderService orderService, IUrlHelperFactory urlHelperFactory,IVnPayService vnPayService)
        {
            _orderService = orderService;
            _urlHelperFactory = urlHelperFactory;
            _vnPayService = vnPayService;
        }
        [Authorize()]
        [HttpPost("CheckoutWithNhanHang")]
        public async Task<IActionResult> CheckoutWithNhanHang(CreateOrderDTO payload) { 
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);

              
                var result = await _orderService.Create(payload, userId);
                return Ok(new BaseResponseModel(result));
            
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [Authorize()]
        [HttpPost("CheckoutWithVNpay")]
        public async Task<IActionResult> CheckoutWithVNpay(CreateOrderDTO payload)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);


                var url =  _vnPayService.CreatePaymentUrl(payload, HttpContext, userId);
                return Ok(new { PaymentUrl = url });

            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpGet("PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            
            return Ok(response); 
        }
        [HttpGet("OrderConfirmation")]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            try
            {
                var result = await _orderService.GetById(id);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
