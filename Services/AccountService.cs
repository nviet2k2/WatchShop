using AutoMapper;
using Core.Domains;
using Core.QueryModels;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Generators;
using Repositories;
using Services;
using Services.DTOs;
using Services.Helpter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


namespace Services
{
    public interface IUserService
    {
        Task<PaginationSetModel<UserDTO>> GetAll(PaginationQueryModel payload);
        Task<bool> RegisterUserAsync(RegisterDTO model);
        Task<(bool, string)> SendForgotPasswordEmailAsync(string email);
        Task<bool> VerifyVerificationCodeAsync(string email, string verificationCode);
        Task<string> ResetPasswordAsync(string email, string newPassword);
        Task<string> SendEmailAsync(string email, string subject, string message);
        Task<UserDTO> GetById(int id);
        Task<CreateUserDTO> CreateUserAsync(CreateUserDTO model);
        Task<CreateUserDTO> Update(CreateUserDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
        Task<bool> ForgotPasswordAsync(string email);
    }
}
    public class UserService : IUserService
    {

    private readonly IEmailService _mailService;
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string> _verificationCodes;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper,IRoleRepository roleRepository,ICustomerRepository customerRepository, IConfiguration configuration, ITokenService tokenService, IEmailService mailService)
        {
         _mapper = mapper;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        _roleRepository = roleRepository;
            _configuration = configuration;
            _verificationCodes = new Dictionary<string, string>();
            _tokenService = tokenService;
        _mailService = mailService;
        }
    public async Task<UserDTO> GetById(int id)
    {
        var data = await _userRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Item not found");
        }

        Console.WriteLine("CustomerId: " + data.CustomerId);  // Corrected line
        return _mapper.Map<UserDTO>(data);
    }

    public async Task<PaginationSetModel<UserDTO>> GetAll(PaginationQueryModel queryModel)
    {
        try
        {
            Expression<Func<UserModel, bool>> baseFilter = f => true;

            var data = await _userRepository.GetMulti(baseFilter);
            var mappedData = _mapper.Map<List<UserModel>, List<UserDTO>>(data);


            return new PaginationSetModel<UserDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> SendEmailAsync(string email, string subject, string message)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
        {
            return "Invalid input parameters"; // Handle invalid input parameters gracefully
        }

        Mailrequest mailrequest = new Mailrequest();
        mailrequest.ToEmail = email;
        mailrequest.Subject = subject;
        mailrequest.Body = message;

        try
        {
            // Send the email 
            await _mailService.SendEmailAsync(mailrequest);
            return "Email sent successfully";
        }
        catch (Exception ex)
        {
            // Handle the exception or log it for debugging
            return $"Failed to send email: {ex.Message}";
        }
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
      
       

        //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);

        //await _emailSender.SendEmailAsync(user.Email, "Reset Password",
        //    $"Please reset your password by clicking here: {callbackUrl}");

        return true;
    }
    public async Task<string> ResetPasswordAsync(string email, string newPassword)
    {
        // Kiểm tra xem email và token có hợp lệ không
        var user = await _userRepository.GetSingleByCondition(u => u.Email == email);
        if (user == null)
        {
            return "Người dùng không tồn tại.";
        }

        //var isTokenValid = _tokenService.ValidateToken(token, user);
        //if (!isTokenValid)
        //{
        //    return "Token không hợp lệ hoặc đã hết hạn.";
        //}

        // Xác thực token thành công, đặt lại mật khẩu
        user.PasswordSalt = newPassword; // Hàm HashPassword để mã hóa mật khẩu
        _userRepository.Update(user);
        await _userRepository.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

        return "Mật khẩu đã được đặt lại thành công.";
    }

    private string HashPassword(string password)
    {
        // Thực hiện mã hóa mật khẩu, ví dụ: sử dụng BCrypt.Net để bcrypt mật khẩu
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<(bool, string)> SendForgotPasswordEmailAsync(string email)
        {
            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();
            _verificationCodes[email] = verificationCode;

            

            return (true, verificationCode);
        }
        public async Task<bool> VerifyVerificationCodeAsync(string email, string verificationCode)
        {
            if (_verificationCodes.TryGetValue(email, out string storedCode) && storedCode == verificationCode)
            {
                _verificationCodes.Remove(email);
                return true;
            }

            return false;
        }
        public async Task<bool> RegisterUserAsync(RegisterDTO model)
        {
            // Kiểm tra xem email đã được sử dụng chưa
            if (await _userRepository.AnyAsync(u => u.Email == model.Email&& u.DisplayName == model.DisplayName))
            {
                return false; // Trả về false nếu email đã được sử dụng
            }
        var random = new Random();
        var verificationCode = random.Next(100000, 999999).ToString();
        var customer = new CustomerModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Gender = model.Gender,
            DateOfBirth =model.DateOfBirth,
            Code = verificationCode,
        };
        _customerRepository.Add(customer);
        await _customerRepository.SaveChanges();
        string salt = await GenerateSalt();
        var user = new UserModel
        {
            Id = model.Id,
            Email = model.Email,
           
            DisplayName = model.DisplayName,
            RoleId = 2,
            PhoneNumber = model.PhoneNumber,
            LastOnline = model.LastOnline,
            CustomerId = customer.Id,
            EmployeeId = null,
            PasswordSalt = salt,
            HashedPassword = await PasswordHashing(model.Password, salt)

        };
            _userRepository.Add(user);
            await _userRepository.SaveChanges();
            //var userRole = new UserRoleModel
            //{
            //    RoleId = user.RoleId,
            //    UserId = user.Id,
            //};
            // Kiểm tra xem vai trò có tồn tại không


            // Nếu vai trò không tồn tại, thiết lập mặc định là "customer"
            //var roleEntity = await _roleRepository.GetSingleByCondition(r => r.RoleTitle.ToLower() == "Customer");


            //user.Role = roleEntity;

            //_userRoleRepository.Add(userRole);

            //await _userRoleRepository.SaveChanges();
            return true; // Trả về true nếu đăng ký thành công
        }
    public async Task<CreateUserDTO> CreateUserAsync(CreateUserDTO model)
    {
        // Kiểm tra xem email đã được sử dụng chưa
        try {
            if (await _userRepository.AnyAsync(u => u.Email == model.Email && u.DisplayName == model.DisplayName))
            {
                return null;
            }
            string salt = await GenerateSalt();
            var user = new UserModel
            {
                Id = model.Id,
                Email = model.Email,

                DisplayName = model.DisplayName,
                RoleId =model.RoleId,
                PhoneNumber = model.PhoneNumber,
                LastOnline = model.LastOnline,
                CustomerId = model.CustomerId,
                EmployeeId = model.EmployeeId,
                PasswordSalt = salt,
                HashedPassword = await PasswordHashing(model.Password, salt)

            };
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChanges();
            return _mapper.Map<CreateUserDTO>(user);   


        } catch 
        (Exception ex)
        {
        throw new Exception(ex.Message, ex);
        
        }
        

       
    }
    public async Task<int> Delete(int id)
    {
        var foundItem = _userRepository.FirstOrDefault(x => x.Id == id);
        if (foundItem == null)
        {
            throw new Exception("Item not found");
        }
        try
        {
            _userRepository.Remove(foundItem);
            await _userRepository.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return id;
    }
    public async Task<int[]> DeleteMultiple(int[] ids)
    {
        if (ids == null || ids.Length == 0)
        {
            throw new Exception("No ids provided");
        }

        try
        {
            var itemsToDelete = _userRepository.GetAll().Where(x => ids.Contains(x.Id));

            if (itemsToDelete.Count() == 0)
            {
                throw new Exception("No items found");
            }


            _userRepository.RemoveRange(itemsToDelete);
            await _userRepository.SaveChanges();

            return ids;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<CreateUserDTO> Update(CreateUserDTO payload)
    {
        var data = await _userRepository.FirstOrDefaultAsync(x => x.Id == payload.Id);
        if (data == null)
        {
            throw new Exception($"{payload.Id} was not found");
        }
        data.DisplayName = payload.DisplayName;
        data.PhoneNumber = payload.PhoneNumber;
        data.Email = payload.Email;
        data.RoleId = payload.RoleId;
        await _userRepository.SaveChanges();
        return _mapper.Map<CreateUserDTO>(data);
    }
    private async Task<string> GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
    private async Task<string> PasswordHashing(string password, string salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
        {
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }
    }
}

