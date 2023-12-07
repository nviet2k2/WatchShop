using Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Database
{
    public class WebApiContext : DbContext
    {
        public WebApiContext(DbContextOptions<WebApiContext> options) : base(options) { }
        public DbSet<OrderDetailModel> BillDetails { get; set; }
        public DbSet<OrderModel> BillModels { get; set; }
        public DbSet<BrandModel> BrandModels { get; set; }
        public DbSet<CategoryModel> CategoryModels { get; set; }
        public DbSet<PermissionModel> PermissionModels { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ReceiptDetailModel> ReceiptDetailModels { get; set; }
        public DbSet<ReceiptModel> ReceiptModels { get; set; }
        public DbSet<RoleModel> RoleModels { get; set; }
        public DbSet<SupplierModel> SuppliersModels { get; set; }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<RolePermissionModel> RolePermissionsModels { get; set; }
        public DbSet<VoucherModel> VoucherModels { get; set; }
        public DbSet<OrderPaymentModel> OrderPaymentModels { get; set; }
        public DbSet<OrganizationPersonaTitleModel> OrganizationPersonaTitleModels { get; set; }
        //public DbSet<AccessoryModel> AccessoriesModels { get; set; }
        public DbSet<AddressModel> AddressModels { get; set; }
        public DbSet<ProductReviewModel> ProductReviewModels { get; set; }
        public DbSet<PaymentModel> PaymentModels { get; set; }
        public DbSet<CustomerModel> CustomerModels { get; set; }
        public DbSet<EmployeeModel> EmployeeModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleModel>().HasData(
            new RoleModel { Id = 1, RoleTitle = "Admin", Description = "Admin" },
            new RoleModel { Id = 2, RoleTitle = "Customer", Description = "Customer" },
            new RoleModel { Id = 3, RoleTitle = "Employee", Description = "Employee" }

        );
            //modelBuilder.Entity<UserModel>().HasData(
            //    new UserModel
            //    {
            //        Id = 2,
            //        Email = "admin@example.com",
            //        HashedPassword = "Admin@123", 
            //        PhoneNumber = "1234567890", 
            //        DisplayName = "Admin",
            //        LastOnline = DateTime.Now,
            //        EmployeeId = null,
            //        CustomerId = null, 
            //        RoleId = 1,
            //    });
        }

    }
}