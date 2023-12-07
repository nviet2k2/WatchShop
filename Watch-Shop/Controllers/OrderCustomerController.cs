using Core.Domains;
using Core.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;
using System.Data;
using System.Security.Claims;

namespace Watch_Shop.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderCustomerController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderCustomerController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize()]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel payload)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);

                var result = await _orderService.GetAllCUSTOMER(payload, userId);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GetAllDetail")]
        public async Task<IActionResult> GetAllDetail([FromQuery] PaginationQueryModel payload)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);

                var result = await _orderService.GetAllDetailCUSTOMER(payload, userId);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPut("CanceleOrder/{id}")]
        public async Task<IActionResult> CanceleOrder(int id)
        {
            try
            {


                var result = await _orderService.CanceleOrder(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
