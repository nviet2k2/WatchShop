using AutoMapper;
using Core.Domains;
using Core.QueryModels;
using Database.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{

    public interface IOrderService
    {
        Task<PaginationSetModel<ListOrderDetailDTO>> GetAllDetailCUSTOMER(PaginationQueryModel queryModel, int id);
        Task<PaginationSetModel<OrderDTO>> GetAllCUSTOMER(PaginationQueryModel queryModel, int id);
        Task<PaginationSetModel<ListOrderDetailDTO>> GetAllDetailADMIN(PaginationQueryModel queryModel);
        Task<PaginationSetModel<OrderDTO>> GetAllADMIN(PaginationQueryModel queryModel);
        Task<OrderDTO> CanceleOrder(int id);
        Task<OrderDTO> UpdateStatus(OrderDTO payload);
        Task<string> UpdateOrder(string orderId, string newStatus, double amount);
        //Task<string> UpdateOrder(string orderId, int userId, string newStatus,double amount);
        Task<CreateOrderDTO> Create(CreateOrderDTO payload, int UserId);
        Task<byte[]> ExportExcel();

        //Task<BrandDTO> Update(BrandDTO payload);
        //Task<int> Delete(int id);
        Task<OrderDTO> GetById(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderDetailRepository _detailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentOrderRepository _paymentOrderRepository;
       private readonly IMapper _mapper;
        public OrderService(IOrderRepository repository, IMapper mapper, IOrderDetailRepository detailRepository, IProductRepository productRepository, IPaymentRepository paymentRepository, IPaymentOrderRepository paymentOrderRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _detailRepository = detailRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
            _paymentOrderRepository = paymentOrderRepository;
        }

        public async Task<PaginationSetModel<ListOrderDetailDTO>> GetAllDetailADMIN(PaginationQueryModel queryModel)
        {
            try
            {
                var data = _detailRepository
                    .GetAll().ToList();
                var mappedData = _mapper.Map<List<OrderDetailModel>, List<ListOrderDetailDTO>>(data);

                return new PaginationSetModel<ListOrderDetailDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PaginationSetModel<OrderDTO>> GetAllADMIN(PaginationQueryModel queryModel)
        {
            try
            {
                var data = _repository
                    .GetAll().ToList();
                var mappedData = _mapper.Map<List<OrderModel>, List<OrderDTO>>(data);

                return new PaginationSetModel<OrderDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PaginationSetModel<OrderDTO>> GetAllCUSTOMER(PaginationQueryModel queryModel, int id)
        {
            try
            {
                var data = _repository
                    .GetAll().Where(x => x.UserId == id).ToList();
                var mappedData = _mapper.Map<List<OrderModel>, List<OrderDTO>>(data);

                return new PaginationSetModel<OrderDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<OrderDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<OrderDTO>(data);
        }
        public async Task<CreateOrderDTO> Create(CreateOrderDTO payload, int UserId)
        {
            var random = new Random();
            var randomNumber = random.Next(1, 100);
            var pay = new PaymentModel
            {

                PaymentCode = randomNumber.ToString(),
                PaymentMethod = "Thanh toán khi nhận hàng",
                transactionId = randomNumber.ToString(),
                status = "Đã thanh toán"
            };
            await _paymentRepository.AddAsync(pay);
            await _paymentRepository.SaveChanges();
            var data = new OrderModel
            {
                Id = payload.Id,
                OrderStatus = "Đang xử lý",
                DeliverAddress = payload.DeliverAddress,
                VoucherId = payload.VoucherId,
                UserId = UserId,
                Note = payload.Note,
                OrderCode = randomNumber.ToString(),
                CreateDate = DateTime.UtcNow,
            };
           
            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
                var orderPayment = new OrderPaymentModel
                {
                  
                    OrderId =data.Id,
                    PaymentId = pay.Id,
                };
                await _paymentOrderRepository.AddAsync(orderPayment);
                await _paymentRepository.SaveChanges();
                foreach (var item in payload.details)
                {
                    var datadetail = _mapper.Map<OrderDetailModel>(item);
                    datadetail.BillId = data.Id;
                    await _detailRepository.AddAsync(datadetail);
                    await _detailRepository.SaveChanges();
                    var dataproduct = _productRepository.FirstOrDefault(x => x.Id == datadetail.ProductId);
                    if(dataproduct != null) {
                        dataproduct.Quantity -= datadetail.Quantity;
                        if (dataproduct.Quantity < 0)
                        {
                            dataproduct.Quantity = 0; 
                        }
                        _productRepository.Update(dataproduct);
                        await _productRepository.SaveChanges();
                    }
                    else
                    {
                        throw new Exception($"{dataproduct.Id} was not found");
                    }
                   
                }

                data.TotalPrice = payload.TotalPrice;
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<CreateOrderDTO>(data);
        }

        public async Task<OrderDTO> CanceleOrder(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception($"{id} was not found");
            }
            if (data.OrderStatus == "Đang xử lý")
            {
                data.OrderStatus = "Đã hủy";
            }
            else
            {
                throw new Exception($"Đơn hàng có thể đang giao hoặc hoàn thành hãy kiểm tra lại");
            }
            await _repository.SaveChanges();
            return _mapper.Map<OrderDTO>(data);
        }
        public async Task<string> UpdateOrder(string orderId, string newStatus, double amount)
        {

            var data = await _repository.FirstOrDefaultAsync(x => x.OrderCode == orderId);
            if (data == null)
            {
                throw new Exception("Không tìm thấy đơn hàng");
            }
            try
            {
                data.OrderStatus = newStatus;
                data.TotalPrice -= amount;

                var random = new Random();
                var randomNumber = random.Next(1, 100);
                var pay = new PaymentModel
                {
                    PaymentCode = randomNumber.ToString(),
                    PaymentMethod = "Thanh toán Vnpay",
                    transactionId = randomNumber.ToString(),
                    status = "Đã thanh toán"
                };

                await _paymentRepository.AddAsync(pay);
                await _paymentRepository.SaveChanges();
                var orderPayment = new OrderPaymentModel
                {

                    OrderId = data.Id,
                    PaymentId = pay.Id,
                };
                await _paymentOrderRepository.AddAsync(orderPayment);
                await _paymentRepository.SaveChanges();
                _repository.Update(data);
                await _repository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return "Đã thành toán";
        }

        public async Task<string> UpdateOrder(string orderId,int userId, string newStatus, double amount)
        {
            
            var data = await _repository.FirstOrDefaultAsync(x => x.OrderCode == orderId&&x.UserId== userId);

            if (data != null)
            {
                data.OrderStatus = newStatus;
                data.TotalPrice -= amount;
                await _repository.SaveChanges();
                var random = new Random();
                var randomNumber = random.Next(1, 100);
                var pay = new PaymentModel
                {
                    PaymentCode = randomNumber.ToString(),
                    PaymentMethod = "Thanh toán Vnpay",
                    transactionId = randomNumber.ToString(),
                    status = "Đã thanh toán"
                };
                await _paymentRepository.AddAsync(pay);
                await _paymentRepository.SaveChanges();
                var orderPayment = new OrderPaymentModel
                {

                    OrderId = data.Id,
                    PaymentId = pay.Id,
                };
                await _paymentOrderRepository.AddAsync(orderPayment);
                await _paymentRepository.SaveChanges();
                return "đã thanh toán";
            }
            else
            {
                throw new Exception("Không tồn tại đơn hàng này");
            }
        }

        public async Task<OrderDTO> UpdateStatus(OrderDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);

            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }

            if (data.OrderStatus != "Hoàn thành")
            {

                data.OrderStatus = payload.OrderStatus;
            }
            else
            {
                throw new Exception($"Order {payload.Id} đã hoàn thành.Không thể cập nhật trạng thái");
            }
            _repository.Update(data);
            await _repository.SaveChanges();
            return _mapper.Map<OrderDTO>(data);
        }
        public async Task<byte[]> ExportExcel()
        {
            try
            {
                var orders = _repository.GetAll().ToList();
                var orderDetails = _detailRepository.GetAll().ToList();
                var products = _productRepository.GetAll().ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Orders and Details");
                    worksheet.Columns[1].Style.Font.Bold = true;

                    // Add order headers
                    worksheet.Cells[1, 1].Value = "Mã đơn hàng";
                    worksheet.Cells[1, 2].Value = "Ngày đặt hàng";
                    worksheet.Cells[1, 3].Value = "Tổng tiền";

                    // Add order detail headers
                    worksheet.Cells[1, 4].Value = "Mã sản phẩm";
                    worksheet.Cells[1, 5].Value = "Tên sản phẩm ";
                    worksheet.Cells[1, 6].Value = "Price";
                    worksheet.Cells[1, 7].Value = "Quantity";

                    int row = 2;
                    foreach (var order in orders)
                    {
                        // Add order information
                        worksheet.Cells[row, 1].Value = order.Id;
                        worksheet.Cells[row, 2].Value = order.CreateDate;

                        int startRow = row;

                        foreach (var product in products)
                        {
                            var orderDetailGroups = orderDetails
                                .Where(od => od.BillId == order.Id && od.ProductId == product.Id)
                                .ToList();

                            foreach (var orderDetailGroup in orderDetailGroups)
                            {
                                foreach (var orderDetail in orderDetailGroups)
                                {
                                    worksheet.Cells[row, 4].Value = orderDetail.ProductId;
                                    worksheet.Cells[row, 5].Value = orderDetail.Product?.ProductName;
                                    worksheet.Cells[row, 6].Value = orderDetail.UnitPrice;
                                    worksheet.Cells[row, 7].Value = orderDetail.Quantity;

                                    row++;
                                }

                                // Merge cells for TotalPrice column
                                worksheet.Cells[row - orderDetailGroups.Count(), 3, row - 1, 3].Merge = true;
                                worksheet.Cells[row - 1, 3].Value = order.TotalPrice;
                                var mergedCell = worksheet.Cells[startRow, 3, row - 1, 3];
                                mergedCell.Merge = true;

                                // Center-align the content in the merged cell
                                mergedCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                row++;
                            }
                        }
                    }

                    // Autofit columns
                    worksheet.Cells.AutoFitColumns();

                    // Convert package to byte array for download
                    var excelBytes = package.GetAsByteArray();
                    return excelBytes;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }



        public async Task<PaginationSetModel<ListOrderDetailDTO>> GetAllDetailCUSTOMER(PaginationQueryModel queryModel, int id)
        {

            try
            {
                var data = _detailRepository
                    .GetAll()
                    .Where(x => x.Bill.UserId == id)
                    .ToList();  // Execute the query to retrieve the data from the database

                var mappedData = _mapper.Map<List<OrderDetailModel>, List<ListOrderDetailDTO>>(data);

                return new PaginationSetModel<ListOrderDetailDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}
