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
   
    public interface ISupplierService
    {
        Task<PaginationSetModel<SupplierDTO>> GetAll(PaginationQueryModel payload);
        Task<SupplierDTO> GetById(int id);
        Task<SupplierDTO> Create(SupplierDTO payload);
        Task<SupplierDTO> Update(SupplierDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);

    }
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;
        private readonly IMapper _mapper;
        public SupplierService(ISupplierRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
        public async Task<SupplierDTO> Create(SupplierDTO payload)
        {
            var data = _mapper.Map<SupplierModel>(payload);
            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<SupplierDTO>(data);
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
        public async Task<PaginationSetModel<SupplierDTO>> GetAll(PaginationQueryModel payload)
        {
            try
            {
                Expression<Func<SupplierModel, bool>> baseFilter = f => true;

                var data = await _repository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<SupplierModel>, List<SupplierDTO>>(data);


                return new PaginationSetModel<SupplierDTO>(payload.PageNumber, payload.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       

        public async Task<SupplierDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<SupplierDTO>(data);
        }

        public async Task<SupplierDTO> Update(SupplierDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
            data.Name = payload.Name;
            data.PhoneNumber = payload.PhoneNumber;
            data.Email = payload.Email;
            data.Address   = payload.Address;
            
            await _repository.SaveChanges();
            return _mapper.Map<SupplierDTO>(data);
        }

    }
}
