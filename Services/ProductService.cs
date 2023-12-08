using AutoMapper;
using Core.Domains;
using Core.QueryModels;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public interface IProductService
    {
        Task<PaginationSetModel<ProductDTO>> GetAllProduct(PaginationQueryModel queryModel);
        Task<ProductDTO> GetById(int id);
        Task<CreateProductDTO> Create(CreateProductDTO payload, IFormFile[] Images, IFormFile Thumnail);
        Task<CreateProductDTO> Update(CreateProductDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
        Task<byte[]> ExportExcel();
        Task<string> ImportExcel(IFormFile file);
        Task<ProductDTO> GetProductByNameAsync(int categoryId,int brandId);


    }
    public class ProductService : IProductService
    {
      
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repository, IMapper mapper, ILogger<ProductService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;


        }
        public async Task<CreateProductDTO> Create(CreateProductDTO payload, IFormFile[] Images, IFormFile Thumnail)
        {
          
            _logger.LogInformation("{Timestamp} [{Provider}] Create service is running", DateTime.Now, "ProductService");
            var data = _mapper.Map<ProductModel>(payload);
            bool isCodeUnique = false;
            string productCode = "";
             
            if (!isCodeUnique)
            {
                var random = new Random();
                var randomNumber = random.Next(1, 100);
                productCode = $"SP{randomNumber:00}{Guid.NewGuid().ToString().Substring(0, 5)}";

               
                isCodeUnique = await _repository.AnyAsync(p => p.Code == productCode);
            }
            data.Code = productCode;
            // Danh sách để lưu đường dẫn của các hình ảnh
            var imagePaths = new List<string>();

            // Xử lý hình ảnh từ files
            if (Images != null)
            {
                foreach (var file in Images)
                {
                    if (file.Length > 0)
                    {
                        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExtension = Path.GetExtension(file.FileName);
                        var currentTime = DateTime.Now.ToString("yyyyMMddHHmmssfffttt");
                        var fileName = $"{originalFileName}_{currentTime}{fileExtension}";
                        var uploadFolderPath = Path.Combine("wwwroot", @"Anh\product");
                        var filePath = Path.Combine(uploadFolderPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        var fullFilePath = "https://localhost:7199/" + "Anh/product/" + fileName;

                        
                        imagePaths.Add(fullFilePath);
                    }
                }
            }

            // Xử lý filee
            if (Thumnail.Length > 0)
            {
                var originalFileName = Path.GetFileNameWithoutExtension(Thumnail.FileName);
                var fileExtension = Path.GetExtension(Thumnail.FileName);
                var currentTime = DateTime.Now.ToString("yyyyMMddHHmmssfffttt");
                var fileName = $"{originalFileName}_{currentTime}{fileExtension}";
                var uploadFolderPath = Path.Combine("wwwroot", @"Anh\Thumnail");
                var filePath = Path.Combine(uploadFolderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Thumnail.CopyToAsync(fileStream);
                }

                var fullFilePath = "https://localhost:7199/" + "Anh/Thumnail/" + fileName;

                
                data.Thumnail = fullFilePath;
            }
            else
            {
                data.Thumnail = "";
            }

            
            data.Img = string.Join(",", imagePaths);

            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _mapper.Map<CreateProductDTO>(data);
        }

        public async Task<int[]> DeleteMultiple(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new Exception("No ids provided");
            }

            try
            {
                var itemsToDelete = _repository.GetAll().Where(x => ids.Contains(x.Id));

                if (itemsToDelete.Count() == 0)
                {
                    throw new Exception("No items found");
                }

                // Delete corresponding image files
                foreach (var item in itemsToDelete)
                {
                    // Combine Thumbnail and Img properties to form complete file paths
                    var thumbnailPath = Path.Combine("wwwroot", "Anh", "Thumnail", item.Thumnail);
                    var imagePaths = item.Img.Split(',');

                    foreach (var imagePath in imagePaths)
                    {
                        var mainImagePath = Path.Combine("wwwroot", "Anh", "product", imagePath);

                        // Check and delete the thumbnail image
                        if (File.Exists(thumbnailPath))
                        {
                            File.Delete(thumbnailPath);
                        }

                        // Check and delete the main image
                        if (File.Exists(mainImagePath))
                        {
                            File.Delete(mainImagePath);
                        }
                    }
                }

                _repository.RemoveRange(itemsToDelete);
                await _repository.SaveChanges();

                return ids;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> Delete(int id)
        {
            var foundItem = _repository.FirstOrDefault(x => x.Id == id);
            if (foundItem == null)
            {
                throw new Exception("Item not found");
            }
            try
            {
                _repository.Remove(foundItem);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;
        }

        public async Task<PaginationSetModel<ProductDTO>> GetAllProduct(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<ProductModel, bool>> baseFilter = f => true;


                var data = await _repository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<ProductModel>, List<ProductDTO>>(data);


                return new PaginationSetModel<ProductDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    

        public async Task<ProductDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<ProductDTO>(data);
        }
        public async Task<ProductDTO> GetProductByNameAsync( int categoryId, int brandId)
        {
            
            var product = await _repository
                .FirstOrDefaultAsync(p=>p.CategoryId==categoryId||p.CategoryId==categoryId);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<CreateProductDTO> Update(CreateProductDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
            data.ProductName= payload.ProductName;
            data.BrandId = payload.BrandId;
            data.CategoryId = payload.CategoryId;
            data.Size = payload.Size;
            data.Color = payload.Color;
            //if (payload.Image != null && payload.Image.Count > 0)
            //{
            //    var imagePaths = new List<string>();

            //    foreach (var imageFile in payload.Image)
            //    {
            //        if (imageFile.Length > 0)
            //        {
            //            var fileName = Path.GetFileName(imageFile.FileName);
            //            var uniqueFileName = Guid.NewGuid() + "_" + fileName;
            //            var path = Path.Combine(Directory.GetCurrentDirectory(), "Utility", "Anh", uniqueFileName);

            //            using (var stream = new FileStream(path, FileMode.Create))
            //            {
            //                await imageFile.CopyToAsync(stream);
            //            }


            //            imagePaths.Add("/Anh/" + uniqueFileName);
            //        }
            //    }
            //    data.Img = string.Join(",", imagePaths);
            //}
            //else
            //{
            //    data.Img = data.Img;
            //}


            //data.Img = payload.Img;        
            await _repository.SaveChanges();
            return _mapper.Map<CreateProductDTO>(data);
        }
        public async Task<byte[]> ExportExcel()
        {
            try
            {
                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

                var data = _repository.GetAll();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Products");
                    worksheet.Columns[1].Style.Font.Bold = true;
                    // Thêm tiêu đề cột
                    worksheet.Cells[1, 1].Value = "Id";
                    worksheet.Cells[1, 2].Value = "Product Name";
                    worksheet.Cells[1, 3].Value = "Img";
                    worksheet.Cells[1, 4].Value = "Price";
                    worksheet.Cells[1, 5].Value = "Quantity";
                    worksheet.Cells[1, 6].Value = "BrandId";
                    worksheet.Cells[1, 7].Value = "Size";
                    worksheet.Cells[1, 8].Value = "Thumnail";
                    worksheet.Cells[1, 9].Value = "Color";
                    worksheet.Cells[1, 10].Value = "Description";
                    worksheet.Cells[1, 11].Value = "Code";
                    worksheet.Cells[1, 12].Value = "Gender";
                    worksheet.Cells[1, 13].Value = "Status";
                    worksheet.Cells[1, 14].Value = "CategoryId";
                   

                    // Thêm dữ liệu sản phẩm vào worksheet
                    int row = 2;
                    foreach (var product in data)
                    {
                        worksheet.Cells[row, 1].Value = product.Id;
                        worksheet.Cells[row, 2].Value = product.ProductName;
                        worksheet.Cells[row, 3].Value = product.Img;
                        worksheet.Cells[row, 4].Value = product.Price;
                        worksheet.Cells[row, 5].Value = product.Quantity;
                        worksheet.Cells[row, 6].Value = product.BrandId;
                        worksheet.Cells[row, 7].Value = product.Size;
                        worksheet.Cells[row, 8].Value = product.Thumnail;
                        worksheet.Cells[row, 9].Value = product.Color;
                        worksheet.Cells[row, 10].Value = product.Description;
                        worksheet.Cells[row, 11].Value = product.Code;
                        worksheet.Cells[row, 12].Value = product.Gender;
                        worksheet.Cells[row, 13].Value = product.Status;
                        worksheet.Cells[row, 14].Value = product.CategoryId;
                       
                        row++;
                    }

                    // Autofit cột
                    worksheet.Cells.AutoFitColumns();

                    // Chuyển đổi package thành mảng byte để tải xuống
                    var excelBytes = package.GetAsByteArray();
                    return excelBytes;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<string> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                throw new Exception("Invalid file format");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var allowedExtensions = new[] { ".xls", ".xlsx", ".csv" };

            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Invalid file format");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    var tests = new List<ProductDTO>();
                    var uniqueProductNames = new HashSet<string>();

                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var productName = worksheet.Cells[row, 2].Value?.ToString();
                        var CategoryId = Convert.ToInt32(worksheet.Cells[row, 14].Value);
                        var BrandId = Convert.ToInt32(worksheet.Cells[row, 6].Value);
                        var productKey = $"{productName}_{CategoryId}_{BrandId}";
                        if (!string.IsNullOrWhiteSpace(productName) && !uniqueProductNames.Contains(productKey))
                        {

                            // Thêm bộ ba giá trị vào HashSet để đảm bảo không thêm sản phẩm trùng lặp
                            uniqueProductNames.Add(productKey);

                            var test = new ProductDTO
                            {
                                ProductName = productName,
                                Img = worksheet.Cells[row, 3].Value?.ToString(),
                                Price = Convert.ToDouble(worksheet.Cells[row, 4].Value),
                                Quantity = Convert.ToInt32(worksheet.Cells[row, 5].Value),
                              
                                Size = worksheet.Cells[row, 7].Value?.ToString(),
                                Thumnail = worksheet.Cells[row, 8].Value?.ToString(),
                                Color = worksheet.Cells[row, 9].Value?.ToString(),
                                Description = worksheet.Cells[row, 10].Value?.ToString(),
                                Code = worksheet.Cells[row, 11].Value?.ToString(),
                                Gender = worksheet.Cells[row, 12].Value?.ToString(),
                                Status = worksheet.Cells[row, 13].Value?.ToString(),
                               
                            };
                            tests.Add(test);
                        }
                    }

                    try
                    {
                        var testDataList = _mapper.Map<List<ProductDTO>, List<ProductModel>>(tests);

                        foreach (var testData in testDataList)
                        {
                            await _repository.AddAsync(testData);
                        }

                        await _repository.SaveChanges();

                        return "Import thành công";
                    }
                    catch (Exception ex)
                    {
                        return $"Import thất bại: {ex.Message}";
                    }
                }
            }
        }

        private string GetHtmlcontent(string subject)
        {
            string imageHtml = @"<img src='cid:product_image' style='width: 25%'>";
            string response = $@"
        <div style='width:100%;padding:5px;margin:5px;border:1px solid #ccc;Background:#ffff;text-align:center'>
            <h1>{subject}</h1>
            {imageHtml}
            <h2>Thank you for working with us.</h2>
        </div>";

            return response;
        }

    }
       



    }
