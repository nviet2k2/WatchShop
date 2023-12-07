using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        //public string HashedPassword { get; set; }
        //public string PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastOnline { get; set; } = DateTime.Now;
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int RoleId { get; set; }
       
    }
    public class CreateUserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastOnline { get; set; } = DateTime.Now;
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int RoleId { get; set; }

    }
    public class LoginDTO
    {
        public string Username { get; set; }
        public string? Password { get; set; }
    }
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
    public class VerificationCodeModel
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
    public class RegisterDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; } //1: Male, 2: Female, 3: Others
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime? LastOnline { get; set; }
    }
}
