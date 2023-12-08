using AutoMapper;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using Repositories;
using Services.DTOs;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using MimeKit;
using System.Linq;
using Core.Domains;
using Core.QueryModels;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public interface IReceiptService
    {
        Task<CreateReceiptDTO> CreateReceipt(CreateReceiptDTO receiptDTO,int userId);
        Task IncreaseProductQuantity(int productId, int quantity);
        Task<PaginationSetModel<ReceiptDTO>> GetAll(PaginationQueryModel queryModel);
        Task<PaginationSetModel<ReceiptDetailDTO>> GetAllDetail(PaginationQueryModel queryModel);
        Task<ReceiptDTO> GetById(int id);
        Task<ReceiptDTO> Update(ReceiptDTO payload);
        Task<int> Delete(int id);
        Task<byte[]> GeneratePDFAsync(int id);
        Task<string>GetProductNameAsync(int? productId);
        Task<int[]> DeleteMultiple(int[] ids);
       
    }

    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailRepository _receiptDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public ReceiptService(IReceiptRepository receiptRepository,IEmployeeRepository employeeRepository,IUserRepository userRepository, IReceiptDetailRepository receiptDetailRepository, IMapper mapper,IProductRepository productRepository)
        {
            _receiptRepository = receiptRepository;
            _receiptDetailRepository = receiptDetailRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task IncreaseProductQuantity(int productId, int quantity)
        {
           
        }
        public async Task<CreateReceiptDTO> CreateReceipt(CreateReceiptDTO receiptDTO, int userId)
        {
            var receiptModel = new ReceiptModel { 
            UserId = userId,
            SupplierId = receiptDTO.SupplierId,
            CreateDate = receiptDTO.CreateDate,
            };
            await _receiptRepository.AddAsync(receiptModel);
            try
            {
                double total = 0;
                await _receiptRepository.AddAsync(receiptModel);
                await _receiptRepository.SaveChanges();

                foreach (var detail in receiptDTO.ReceiptDetails)
                {
                    
                    var receiptDetailModel = _mapper.Map<ReceiptDetailModel>(detail);
                    receiptDetailModel.ReceiptId = receiptModel.Id;
                    await _receiptDetailRepository.AddAsync(receiptDetailModel);
                    await _receiptDetailRepository.SaveChanges();
                    var dataproduct = await _productRepository.FirstOrDefaultAsync(p => p.Id == detail.ProductId);
                    if (dataproduct != null)
                    {
                        dataproduct.Quantity += detail.Quantity;
                        dataproduct.Price = detail.Price * 1.15;
                    }
                    else
                    {
                        throw new Exception("Product not found");
                    }
                    double subtotal = detail.Quantity * detail.Price;
                    total += subtotal;
                    //var product = await _receiptDetailRepository.FirstOrDefaultAsync(p => p.ProductId == productId);
                    //if (product != null)
                    //{
                    //    product.Quantity += quantity;
                    //}
                    //else
                    //{
                    //    throw new Exception("Product not found");
                    //}

                }
                receiptModel.Total = total;
                await _receiptRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<CreateReceiptDTO>(receiptModel);
        }
        public async Task<int[]> DeleteMultiple(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentException("No ids provided");
            }

            try
            {
                // Fetch items to delete from the repositories
                var itemsToDelete = _receiptRepository.GetAll().Where(x => ids.Contains(x.Id)).ToList();
                var itemsDetailToDelete = _receiptDetailRepository.GetAll()
     .Where(x => ids.Contains(x.ReceiptId))
     .ToList();


                if (itemsToDelete.Count == 0 && itemsDetailToDelete.Count == 0)
                {
                    throw new Exception("No items found");
                }

                // Remove items from the repositories
                if (itemsDetailToDelete.Count > 0)
                {
                    _receiptDetailRepository.RemoveRange(itemsDetailToDelete);
                    await _receiptDetailRepository.SaveChanges();
                }

                if (itemsToDelete.Count > 0)
                {
                    _receiptRepository.RemoveRange(itemsToDelete);
                    await _receiptRepository.SaveChanges();
                }

                return ids;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting items: {ex.Message}", ex);
            }
        }

        public async Task<int> Delete(int id)
        {
            var foundItem = _receiptRepository.FirstOrDefault(x => x.Id == id);
            if (foundItem == null)
            {
                throw new Exception("Item not found");
            }
            try
            {
                _receiptRepository.Remove(foundItem);
                await _receiptRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;
        }

        public async Task<IEnumerable<ReceiptDTO>> GetAll()
        {
            var data = _receiptRepository.GetAll();
            return _mapper.Map<IEnumerable<ReceiptDTO>>(data);
        }
        public async Task<PaginationSetModel<ReceiptDTO>> GetAll(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<ReceiptModel, bool>> baseFilter = f => true;


                var data = await _receiptRepository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<ReceiptModel>, List<ReceiptDTO>>(data);


                return new PaginationSetModel<ReceiptDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PaginationSetModel<ReceiptDetailDTO>> GetAllDetail(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<ReceiptDetailModel, bool>> baseFilter = f => true;


                var data = await _receiptDetailRepository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<ReceiptDetailModel>, List<ReceiptDetailDTO>>(data);


                return new PaginationSetModel<ReceiptDetailDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public async Task<byte[]> GeneratePDFAsync(int id)
        {
            var document = new PdfDocument();
           
            var details = await _receiptDetailRepository.FindAsync(x => x.ReceiptId == id);
            var header = await _receiptRepository.FirstOrDefaultAsync(x => x.Id == id);
            
           
            string htmlcontent = "<div style='width:100%; text-align:center'>";

            htmlcontent += "<h2>Hóa đơn Nhập Hàng </h2>";
            if (header != null)
            {
                htmlcontent += "<h2>  Nhân viên có mã tài khoản :"  +(header.UserId) + " & Ngày Nhập:" + header.CreateDate + "</h2>";
                htmlcontent += "<div>";
            }
            htmlcontent += "<table style ='width:100%; border: 1px solid #000'>";
            htmlcontent += "<thead style='font-weight:bold;background:#104484'>";
            htmlcontent += "<tr>";
            htmlcontent += "<td style='border:2px solid #000'> Mã SP </td>";
            htmlcontent += "<td style='border:2px solid #000'>Tên SP</td>";
            htmlcontent += "<td style='border:2px solid #000'>Số lượng</td>";
            htmlcontent += "<td style='border:2px solid #000'>Giá</td>";

            htmlcontent += "</tr>";
            htmlcontent += "</thead>";

            htmlcontent += "<tbody>";
            if (details != null && details.Any())
            {
                foreach (var detail in details)
                {
                    htmlcontent += "<tr>";
                    htmlcontent += "<td style='border: 1px solid #000'>" + detail.ProductId + "</td>";
                    var productName = await GetProductNameAsync(detail.ProductId);
                    htmlcontent += "<td style='border: 1px solid #000'>" + productName + "</td>";
                    htmlcontent += "<td style='border: 1px solid #000'>" + detail.Quantity + "</td>";
                    htmlcontent += "<td style='border: 1px solid #000'>" + detail.Price + "</td>";
                    htmlcontent += "</tr>";
                }
            }
            htmlcontent += "</tbody>";
            htmlcontent += "</table>";

            htmlcontent += "</div>";
            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<h1> Tổng tiền :" +header.Total+"VND</h1>";
           
              
            htmlcontent += "</div>";

            htmlcontent += "</div>";
            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);

            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            return response;
        }

        public async Task<string>GetProductNameAsync(int? productId)
        {
            
            return await _productRepository.GetProductNameAsync(productId);
        }

        public async Task<ReceiptDTO> GetById(int id)
        {
            var data = await _receiptRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<ReceiptDTO>(data);
        }

        public async Task<ReceiptDTO> Update(ReceiptDTO payload)
        {
            var data = await _receiptRepository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
           data.SupplierId =payload.SupplierId;
            await _receiptRepository.SaveChanges();
            return _mapper.Map<ReceiptDTO>(data);
        }

      
    }
}
