using AutoMapper;
using Database.Models;
using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IStatisticsService {
        Task<int> GetTotalOrders();
        Task<Double> GetTotalRevenue();
        Task<int> GetTotalCustomers();
        Task<List<ProductDTO>> GetTopSellingProducts();
    } 
    public class StatisticsService : IStatisticsService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public StatisticsService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<int> GetTotalCustomers()
        {
            return await _customerRepository.GetTotalCustomersAsync();
        }

        public async Task<int> GetTotalOrders()
        {
            return await _orderRepository.GetTotalOrders();
        }

        public async Task<double> GetTotalRevenue()
        {

            return await _orderRepository.GetTotalRevenue();

        }
        public async Task<List<ProductDTO>> GetTopSellingProducts()
        {
            int topCount = 3;
            var data = await _orderRepository.GetTopSellingProducts(topCount);
            return _mapper.Map<List<ProductDTO>>(data);
        }
    }
}
