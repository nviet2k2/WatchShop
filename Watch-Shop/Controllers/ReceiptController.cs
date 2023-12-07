using Core.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services;
using Core.QueryModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Watch_Shop.Controllers
{
    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        public ReceiptController(IReceiptService receiptService)
        {
           _receiptService = receiptService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateReceiptDTO payload)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);

                var result = await _receiptService.CreateReceipt(payload, userId);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GetAllDetail")]
        public async Task<IActionResult> GetAllDetail([FromQuery] PaginationQueryModel queryModel)
        {
            try
            {
                //var paymentUrl = VnPayHelper.CreatePaymentUrl(request);

                // Trả về URL thanh toán cho client
                //return Ok(new { PaymentUrl = paymentUrl });
                var result = await _receiptService.GetAllDetail(queryModel);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel queryModel)
        {
            try
            {
                //var paymentUrl = VnPayHelper.CreatePaymentUrl(request);

                // Trả về URL thanh toán cho client
                //return Ok(new { PaymentUrl = paymentUrl });
                var result = await _receiptService.GetAll(queryModel);
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
                var result = await _receiptService.DeleteMultiple(ids);
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
                var result = await _receiptService.GetById(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GeneratePdf")]
        public async Task<IActionResult> GeneratedPDF(int id)
        {

            try
            {
                byte[] pdfData = await _receiptService.GeneratePDFAsync(id);
                string filename = "DonNhap_" + id + ".pdf";
                return File(pdfData, "application/pdf", filename);
                
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
       

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ReceiptDTO payload)
        {
            try
            {
                var result = await _receiptService.Update(payload);
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
                var result = await _receiptService.Delete(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

    }
}
