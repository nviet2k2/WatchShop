using Core.Domains;
using Core.QueryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;
using System.Security.Claims;

namespace Watch_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderAdminController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel payload)
        {
            try
            {
                var result = await _orderService.GetAllADMIN(payload);
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
                var result = await _orderService.GetAllDetailADMIN(payload);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(OrderDTO payload)
        {
            try
            {
                var result = await _orderService.UpdateStatus(payload);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {

            try
            {

                var result = await _orderService.ExportExcel();
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
