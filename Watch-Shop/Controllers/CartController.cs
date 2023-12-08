using Core.Domains;
using Database.Models;
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
                return Redirect("http://localhost:3000/sucess-cart");
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
        //[Authorize()]
        [HttpGet("paymentCallback")]
        public IActionResult PaymentCallback([FromQuery] Dictionary<string, string> queryParams)
        {
            string vnp_ResponseCode = queryParams.GetValueOrDefault("vnp_ResponseCode");
            string orderId = queryParams.GetValueOrDefault("vnp_TxnRef");
            
              string total = queryParams.GetValueOrDefault("vnp_Amount");
            if (!string.IsNullOrEmpty(orderId))
            {
                if (vnp_ResponseCode=="00")
                {
                    if (double.TryParse(total, out double amount))
                    {
                        try
                        {

                            //var claimsIdentity = (ClaimsIdentity)User.Identity;
                            //var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            //int.TryParse(userIdString, out int userId);

                            //_orderService.UpdateOrder(orderId, userId, "Đã thanh toán", amount);
                            _orderService.UpdateOrder(orderId, "Hoàn thành", amount);
                            return Redirect("http://localhost:3000/sucess-cart");
                        }
                        catch (Exception ex)
                        {
                            return NotFound(ex.Message);
                        }
                    }
                    
                    
                }
                else
                {
                    
                    return Redirect("http://localhost:3000/payment-failed");
                }
            }
            return BadRequest("Không tìm thấy OrderId");
        }
       
    }
}
