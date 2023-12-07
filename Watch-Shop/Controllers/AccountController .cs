using Core.Domains;
using Core.QueryModels;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTOs;
using System.Security.Claims;

namespace Watch_Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
       
        public AccountController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
           
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateUserDTO payload)
        {
            try
            {
                var result = await _userService.CreateUserAsync(payload);

                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CreateUserDTO payload)
        {
            try
            {
                var result = await _userService.Update(payload);
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
                var result = await _userService.DeleteMultiple(ids);
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
                var result = await _userService.Delete(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel payload)
        {
            try
            {
                var result = await _userService.GetAll(payload);
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
                var result = await _userService.GetById(id);
                return Ok(new BaseResponseModel(result));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponseModel(null, false, StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var token = await _authenticationService.AuthenticateAsync(model);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = token });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await _userService.RegisterUserAsync(model);
            if (!result)
            {
                return BadRequest("Email is already in use");
            }
            return Ok("Đăng ký thành công");
        }
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var result = await _userService.ResetPasswordAsync(model.Email, model.NewPassword);

            if (result!=null)
            {
                return Ok(new { message = "Mật khẩu đã được đặt lại thành công." });
            }

            return BadRequest(new { message = result });
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            //var user = await _userManager.FindByEmailAsync(request.Email);
            //if (user != null)
            //{ 

            //}
            var resetToken = Guid.NewGuid().ToString();

            // Gửi email với token đặt lại mật khẩu đến người dùng
            var resetLink = $"https://localhost:7199/api/Account/ResetPassword?email={request.Email}&token={resetToken}";
            var emailSubject = "Đặt lại mật khẩu";
            var emailBody = $"Xin chào {request.Email},<br>Chúng tôi đã nhận được yêu cầu thiết lập lại mật khẩu cho tài khoản của bạn. " +
                $"Vui lòng truy cập vào liên kết dưới đây để đặt lại mật khẩu mới:<br>" +
                $"<a href='{resetLink}'>Đặt lại mật khẩu</a><br>Nếu bạn không yêu cầu thiết lập lại mật khẩu, vui lòng bỏ qua email này.";

            try
            {
                await _userService.SendEmailAsync(request.Email, emailSubject, emailBody);
                return Ok(new { message = "Email đặt lại mật khẩu đã được gửi thành công." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi gửi email
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi gửi email đặt lại mật khẩu." });
            }
        }
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyVerificationCode([FromBody] VerificationCodeModel model)
        {
            var email = model.Email;
            var verificationCode = model.VerificationCode;

            var isCodeValid = await _userService.VerifyVerificationCodeAsync(email, verificationCode);

            if (isCodeValid)
            {
                // Mã xác nhận hợp lệ, chuyển hướng đến chức năng đặt lại mật khẩu hoặc trả về thông báo thành công.
                return Ok(new { Message = "Verification code is valid. Redirect to reset password functionality." });
            }
            else
            {
                return BadRequest(new { Message = "Invalid verification code." });
            }
        }
    }

}
