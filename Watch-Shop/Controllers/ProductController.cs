using Core.Domains;
using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Core.QueryModels;
using Database.Models;

namespace Watch_Shop.Controllers
{
    //[Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]PaginationQueryModel queryModel)
        {
            try
            {    
                var result = await _productService.GetAllProduct(queryModel);
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
                var result = await _productService.GetById(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm]CreateProductDTO payload, IFormFile[] images, IFormFile thumNail)
        {
            try
            {
                var result = await _productService.Create(payload, images, thumNail);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] CreateProductDTO payload)
        {
            try
            {
                var result = await _productService.Update(payload);
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
                var result = await _productService.Delete(id);
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
                var result = await _productService.DeleteMultiple(ids);
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
               
                var result = await _productService.ExportExcel();
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
                {

            try
            {
               
                var result = await _productService.ImportExcel(file);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

    }
}
