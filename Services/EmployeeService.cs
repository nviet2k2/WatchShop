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
   
    public interface IEmployeeService
    {
        Task<PaginationSetModel<EmployeeDTO>> GetAll(PaginationQueryModel payload);
        Task<EmployeeDTO> GetById(int id);
        Task<EmployeeDTO> Create(EmployeeDTO payload);
        Task<EmployeeDTO> Update(EmployeeDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<EmployeeDTO> Create(EmployeeDTO payload)
        {
            var data = _mapper.Map<EmployeeModel>(payload);
            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<EmployeeDTO>(data);
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

        public async Task<PaginationSetModel<EmployeeDTO>> GetAll(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<EmployeeModel, bool>> baseFilter = f => true;

                var data = await _repository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<EmployeeModel>, List<EmployeeDTO>>(data);


                return new PaginationSetModel<EmployeeDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmployeeDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<EmployeeDTO>(data);
        }

        public async Task<EmployeeDTO> Update(EmployeeDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
            data.DateOfBirth = payload.DateOfBirth;
            data.FirstName = payload.FirstName;
            data.LastName = payload.LastName;
            data.FullName  = payload.FullName;
            data.Code = payload.Code;
            data.Email = payload.Email;
            data.Gender = payload.Gender;
            
            await _repository.SaveChanges();
            return _mapper.Map<EmployeeDTO>(data);
        }

    }
}
