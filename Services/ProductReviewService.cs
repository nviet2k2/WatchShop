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
    
  
    public interface IProductReviewService
    {
        Task<IEnumerable<ProductReviewDTO>> GetAll();
        Task<IEnumerable<ProductReviewDTO>> GetAllUser(int userId);
        Task<ProductReviewDTO> GetById(int id);
        Task<CreateProductReviewDTO> Create(CreateProductReviewDTO payload,int userId);
        Task<ProductReviewDTO> Update(ProductReviewDTO payload);
        Task<int> Delete(int id);
        Task<int[]> DeleteMultiple(int[] ids);
    }
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _repository;
        private readonly IMapper _mapper;
        public ProductReviewService(IProductReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateProductReviewDTO> Create(CreateProductReviewDTO payload,int userId)
        {
            var data = new ProductReviewModel { 
             ProductId = payload.ProductId,
             Comment = payload.Comment,
             UserId = userId,
             Rating = payload.Rating,
             Date = payload.Date,
            };
            try
            {
                await _repository.AddAsync(data);
                await _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _mapper.Map<CreateProductReviewDTO>(data);
        }

        public async Task<int> Delete(int id)
        {
            var foundItem = _repository.FirstOrDefault(x => x.ReviewId == id);
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

        public async Task<IEnumerable<ProductReviewDTO>> GetAll()
        {
            try
            {
                var data = _repository.GetAll();   
                return _mapper.Map<IEnumerable<ProductReviewDTO>>(data);    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<ProductReviewDTO>> GetAllUser(int userId)
        {
            try
            {
                var data = _repository.GetAll().Where(x=>x.UserId==userId);
                return _mapper.Map<IEnumerable<ProductReviewDTO>>(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductReviewDTO> GetById(int id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.ReviewId == id);
            if (data == null)
            {
                throw new Exception("Item not found");
            }
            return _mapper.Map<ProductReviewDTO>(data);
        }
        public async Task<int[]> DeleteMultiple(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new Exception("No ids provided");
            }

            try
            {
                var itemsToDelete = _repository.GetAll().Where(x => ids.Contains(x.ReviewId));

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
        public async Task<ProductReviewDTO> Update(ProductReviewDTO payload)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.ReviewId == payload.ReviewId);
            if (data == null)
            {
                throw new Exception($"{payload.ReviewId} was not found");
            }
            data.Comment = payload.Comment;
            data.Rating = payload.Rating;     
            await _repository.SaveChanges();
            return _mapper.Map<ProductReviewDTO>(data);
        }

    }
}
