using AutoMapper;
using Core.Domains;
using Core.QueryModels;
using Database.Models;
using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICustomerService
    {
        Task<PaginationSetModel<CustomerDTO>> GetAll(PaginationQueryModel payload);
        Task<CustomerDTO> GetById(int id);
        Task<CustomerDTO> Update(CustomerDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task<PaginationSetModel<CustomerDTO>> GetAll(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<CustomerModel, bool>> baseFilter = f => true;

                var data = await _repository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<CustomerModel>, List<CustomerDTO>>(data);


                return new PaginationSetModel<CustomerDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CustomerDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<CustomerDTO>(data);
        }

        public async Task<CustomerDTO> Update(CustomerDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
            data.FullName = payload.FullName;
            data.PhoneNumber = payload.PhoneNumber;
            data.DateOfBirth = payload.DateOfBirth;
            data.Email = payload.Email;
            data.Code = payload.Code;
            data.Gender = payload.Gender;
            await _repository.SaveChanges();
            return _mapper.Map<CustomerDTO>(data);
        }

    }
}
