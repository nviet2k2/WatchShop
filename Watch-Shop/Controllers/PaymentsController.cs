using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Paypal;

namespace Watch_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayPalService _payPalService;
        //private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentsController( IHttpContextAccessor httpContextAccessor,IPayPalService payPalService)
        {

          
            _payPalService = payPalService ;
            _httpContextAccessor = httpContextAccessor ;

        }

        [HttpPost("CreatePaymentUrlWithPaypal")]
        public async Task<IActionResult> CreatePaymentUrlWithPaypal(CreateOrderDTO model)
        {
            try
            {
                var url = await _payPalService.CreatePaymentUrl(model, HttpContext);
                return Ok(new { PaymentUrl = url });
            }
            catch (Exception ex)
            {
                
                return BadRequest("An error occurred while processing the payment.");
            }
        }


        [HttpGet("PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _payPalService.PaymentExecute(Request.Query);
            return Ok(response);  
        }

    }
}
