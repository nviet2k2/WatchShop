using AutoMapper;
using Azure;
using Core.Domains;
using Core.QueryModels;
using Database.Models;
using Microsoft.Extensions.Logging;
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
    public interface IBrandService
    {
        Task<PaginationSetModel<BrandDTO>> GetAll(PaginationQueryModel payload);
        Task<BrandDTO> GetById(int id);
        Task<BrandDTO> Create(BrandDTO payload);
        Task<BrandDTO> Update(BrandDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
    }
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IMapper _mapper;
        public BrandService(IBrandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<BrandDTO> Create(BrandDTO payload)
        {
            var data = _mapper.Map<BrandModel>(payload);
            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<BrandDTO>(data);
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

        public async Task<PaginationSetModel<BrandDTO>> GetAll(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<BrandModel, bool>> baseFilter = f => true;

                var data = await _repository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<BrandModel>, List<BrandDTO>>(data);


                return new PaginationSetModel<BrandDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BrandDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<BrandDTO>(data);
        }

        public async Task<BrandDTO> Update(BrandDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
            data.Name = payload.Name;
            await _repository.SaveChanges();
            return _mapper.Map<BrandDTO>(data);
        }

    }
}
