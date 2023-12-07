using Core.Domains;
using Core.QueryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Watch_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;
        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _productReviewService.GetAll();
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [Authorize()]
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);
                var result = await _productReviewService.GetAllUser(userId);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _productReviewService.GetById(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [Authorize()]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateProductReviewDTO payload)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);
                var result = await _productReviewService.Create(payload, userId);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductReviewDTO payload)
        {
            try
            {
                var result = await _productReviewService.Update(payload);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpDelete("delete-multiple")]
        public async Task<IActionResult> DeleteMultipleItems(int[] ids)
        {
            try
            {
                var result = await _productReviewService.DeleteMultiple(ids);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _productReviewService.Delete(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

    }
}
