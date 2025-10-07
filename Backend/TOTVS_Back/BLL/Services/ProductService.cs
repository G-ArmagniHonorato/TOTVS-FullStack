using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.DTOs;
using BLL.Extensions;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDTO>> productGetAll()
        {
            var products = await _repository.GetAll();
            return products.ToDTOList();
        }

        public async Task<ProductDTO?> productGetById(int id)
        {
            var product = await _repository.GetById(id);
            return product?.ToProductDTO();
        }


        public async Task<ProductDTO> productCreate(CreateProductDTO dto)
        {
            var created = await _repository.Create(dto.ToEntity());
            return created.ToProductDTO();
        }

        public async Task<ProductDTO> productUpdate(int id, UpdateProductDTO dto)
        {
            var product = await _repository.GetById(id)
                          ?? throw new KeyNotFoundException($"Produto {id} não encontrado");

            var updated = await _repository.Update(product.ApplyUpdate(dto));
            return updated.ToProductDTO();
        }

        public async Task<bool> productDelete(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null) return false;

            await _repository.Delete(id);
            return true;
        }
    }
}