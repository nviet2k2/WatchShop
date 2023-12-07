using Core.Domains;
using Core.QueryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Watch_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        private IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [HttpGet("TotalOrders")]
        public async Task<IActionResult> TotalOrders()
        {
            try
            {
                var result = await _statisticsService.GetTotalOrders();
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("TotalRevenue")]
        public async Task<IActionResult> TotalRevenue()
        {
            try
            {
                var result = await _statisticsService.GetTotalRevenue();
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("TotalCustomer")]
        public async Task<IActionResult> TotalCustomer()
        {
            try
            {
                int totalCustomers = await _statisticsService.GetTotalCustomers();
                return Ok(new BaseResponseModel(totalCustomers));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GetTopSellingProducts")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            try
            {
                var totalCustomers = await _statisticsService.GetTopSellingProducts();
                return Ok(new BaseResponseModel(totalCustomers));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
