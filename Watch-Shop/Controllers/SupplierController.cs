using Core.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services;
using Core.QueryModels;

namespace Watch_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel queryModel)
        {
            try
            {
                var result = await _supplierService.GetAll(queryModel);
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
                var result = await _supplierService.DeleteMultiple(ids);
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
                var result = await _supplierService.GetById(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SupplierDTO payload)
        {
            try
            {
                var result = await _supplierService.Create(payload);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SupplierDTO payload)
        {
            try
            {
                var result = await _supplierService.Update(payload);
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
                var result = await _supplierService.Delete(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

    }
}
