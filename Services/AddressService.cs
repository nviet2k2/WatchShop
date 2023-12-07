using AutoMapper;
using Core.Domains;
using Core.QueryModels;
using Database.Models;
using Microsoft.EntityFrameworkCore;
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
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAll(int UserId);
        Task<AddressDTO> GetById(int id);
        Task<CreateAddressDTO> Create(CreateAddressDTO payload,int UserId);
        Task<CreateAddressDTO> Update(CreateAddressDTO payload);
        Task<int?> GetCustomerIdByUserIdAsync(int userId);
        Task<int> Delete(int id);
    }
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AddressService(IAddressRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<CreateAddressDTO> Create(CreateAddressDTO payload, int UserId)
        {
            try
            {
                var CustomerId = await GetCustomerIdByUserIdAsync(UserId);

                if (CustomerId.HasValue)
                {
                    var newData = new AddressModel
                    {
                        CustomerId = CustomerId.Value, // Lấy giá trị từ int? (nullable) và gán vào int
                        status = payload.status,
                        Ward = payload.Ward,
                        District = payload.District,
                        HouseNumber = payload.HouseNumber,
                        Note = payload.Note,
                        Province = payload.Province,
                        PhoneNumber = payload.PhoneNumber,
                    };

                    // Đặt tất cả các địa chỉ hiện tại của khách hàng thành false
                    var existingData = _repository.GetAll().Where(x => x.CustomerId == CustomerId && x.status == true);
                    foreach (var existingItem in existingData)
                    {
                        existingItem.status = false;
                    }

                    await _repository.AddAsync(newData);
                    await _repository.SaveChanges();

                    return _mapper.Map<CreateAddressDTO>(newData);
                }
                else
                {
                    throw new Exception("Customer not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int?> GetCustomerIdByUserIdAsync(int userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return user.CustomerId;
            }
            else
            {
                return null;
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

        public async Task<IEnumerable<AddressDTO>> GetAll(int UserId)
        {
            try
            {
                var CustomerId = await GetCustomerIdByUserIdAsync(UserId);
                var data = _repository.GetAll().Where(x=>x.CustomerId== CustomerId);
                return _mapper.Map<IEnumerable<AddressDTO>>(data);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AddressDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<AddressDTO>(data);
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


                _repository.RemoveRange(itemsToDelete);
                await _repository.SaveChanges();

                return ids;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CreateAddressDTO> Update(CreateAddressDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }

            try
            {
               
                var allData = _repository.GetAll().Where(x=> x.status == true);
                foreach (var item in allData)
                {
                    item.status = false;
                }

               
                data.HouseNumber = payload.HouseNumber;
                data.District = payload.District;
                data.Ward = payload.Ward;
                data.Note = payload.Note;
                data.status = true;

               
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
            return _mapper.Map<CreateAddressDTO>(data);
        }


    }
}
