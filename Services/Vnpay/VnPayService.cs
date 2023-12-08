using AutoMapper;
using Core.Domains;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repositories;
using Services.DTOs;

namespace Services.Vnpay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
 
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public VnPayService(IConfiguration configuration, 
            IOrderRepository orderRepository, IOrderDetailRepository
            orderDetailRepository, IMapper mapper,IProductRepository productRepository)
        {
            _configuration = configuration;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
           
            _productRepository = productRepository;


        }
        public async Task<string> CreatePaymentUrl(CreateOrderDTO model, HttpContext context, int UserId)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var data = new OrderModel
            {
                OrderStatus = "Đang xử lý",
                DeliverAddress = model.DeliverAddress,
                Note = model.Note,
                VoucherId = model.VoucherId,
                TotalPrice = model.TotalPrice,
                UserId = UserId,
                CreateDate = model.CreateDate,
            };

            await _orderRepository.AddAsync(data);
            await _orderRepository.SaveChanges();
            foreach (var item in model.details)
            {
                var datadetail = _mapper.Map<OrderDetailModel>(item);
                datadetail.BillId = data.Id;
                await _orderDetailRepository.AddAsync(datadetail);
                await _orderDetailRepository.SaveChanges();
                var dataproduct = _productRepository.FirstOrDefault(x => x.Id == datadetail.ProductId);
                if (dataproduct != null)
                {
                    dataproduct.Quantity -= datadetail.Quantity;
                    _productRepository.Update(dataproduct);
                    await _productRepository.SaveChanges();
                }
                else
                {
                    throw new Exception($"{dataproduct.Id} was not found");
                }
            }
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Note} {model.Note} {model.TotalPrice}");
            pay.AddRequestData("vnp_OrderType", model.Note);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", data.Id.ToString());

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public PaymentResponseVnpayModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            //// Kiểm tra xem response có null không
            //if (response != null)
            //{
            //    // Tạo một đối tượng PaymentModel từ thông tin trong response
            //    var payment = new PaymentModel
            //    {
            //        transactionId = response.TransactionId,
            //        PaymentMethod = response.PaymentMethod,
            //        PaymentCode = response.PaymentId,
            //        // Các thuộc tính khác từ response
            //    };

            //    // Lưu đối tượng PaymentModel vào cơ sở dữ liệu
            //    await _paymentRepository.AddAsync(payment);
            //    await _paymentRepository.SaveChanges();
            //}

            return response;
        }

    }
}

