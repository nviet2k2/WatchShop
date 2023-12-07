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
    public interface IVoucherService
    {
        Task<PaginationSetModel<VoucherDTO>> GetAll(PaginationQueryModel payload);
        Task<VoucherDTO> GetById(int id);
        Task<VoucherDTO> Create(VoucherDTO payload);
        Task<VoucherDTO> Update(VoucherDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
    }
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _repository;
        private readonly IMapper _mapper;
        public VoucherService(IVoucherRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<VoucherDTO> Create(VoucherDTO payload)
        {
            var data = _mapper.Map<VoucherModel>(payload);
            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<VoucherDTO>(data);
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

        public async Task<PaginationSetModel<VoucherDTO>> GetAll(PaginationQueryModel queryModel)
        {
            try
            {
                Expression<Func<VoucherModel, bool>> baseFilter = f => true;

                var data = await _repository.GetMulti(baseFilter);
                var mappedData = _mapper.Map<List<VoucherModel>, List<VoucherDTO>>(data);


                return new PaginationSetModel<VoucherDTO>(queryModel.PageNumber, queryModel.PageSize, mappedData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<VoucherDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<VoucherDTO>(data);
        }

        public async Task<VoucherDTO> Update(VoucherDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == payload.Id);
            if (data == null)
            {
                throw new Exception($"{payload.Id} was not found");
            }
           data.Code = payload.Code;
            data.Value = payload.Value;
            data.StartDate = payload.StartDate;
            data.EndDate = payload.EndDate;
            await _repository.SaveChanges();
            return _mapper.Map<VoucherDTO>(data);
        }

    }
}
