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
    [Authorize()]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService =   addressService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);
                var result = await _addressService.GetAll(userId);
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
                var result = await _addressService.GetById(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateAddressDTO payload)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                int.TryParse(userIdString, out int userId);

                var result = await _addressService.Create(payload, userId);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CreateAddressDTO payload)
        {
            try
            {
                var result = await _addressService.Update(payload);
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
                var result = await _addressService.Delete(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

    }
}
