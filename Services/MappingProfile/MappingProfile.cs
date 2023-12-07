
using AutoMapper;
using Database.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CommonModel, CommonDTO>().ReverseMap();
            CreateMap<TestModel, TestDTO>().ReverseMap();
            CreateMap<ProductModel, ProductDTO>().ReverseMap();
            CreateMap<BrandModel, BrandDTO>().ReverseMap();
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
            CreateMap<ProductModel, CreateProductDTO>().ReverseMap();
            CreateMap<ReceiptModel, ReceiptDTO>().ReverseMap();
            CreateMap<ReceiptModel, CreateReceiptDTO>().ReverseMap();
            CreateMap<ReceiptDetailModel, ReceiptDetailDTO>().ReverseMap();
            CreateMap<ReceiptDetailModel, CreateReceiptDetailDTO>().ReverseMap();
            CreateMap<UserModel, ResetPasswordDTO>().ReverseMap();
            CreateMap<UserModel, ForgotPasswordRequest>().ReverseMap();
            CreateMap<UserModel, UserDTO>().ReverseMap();
            CreateMap<SupplierModel, SupplierDTO>().ReverseMap();
            CreateMap<OrderModel, OrderDTO>().ReverseMap();
            CreateMap<CustomerModel, CustomerDTO>().ReverseMap();
            CreateMap<AddressModel, AddressDTO>().ReverseMap();
            CreateMap<AddressModel, CreateAddressDTO>().ReverseMap();
            CreateMap<ProductReviewModel, ProductReviewDTO>().ReverseMap();
            CreateMap<ProductReviewModel, CreateProductReviewDTO>().ReverseMap();
            CreateMap<EmployeeModel, EmployeeDTO>().ReverseMap();
            CreateMap<UserModel, CreateUserDTO>().ReverseMap();
            CreateMap<OrderModel, CreateOrderDTO>().ReverseMap();
            CreateMap<OrderDetailModel, OrderDetailDTO>().ReverseMap();
            CreateMap<OrganizationPersonaTitleModel, OrganizationPersonaTitleDTO>().ReverseMap();
            CreateMap<VoucherModel, VoucherDTO>().ReverseMap();
        }
    }
}
    

