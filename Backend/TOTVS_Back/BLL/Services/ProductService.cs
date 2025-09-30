using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.DTOs;

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
            return products
                .Where(p => !p.Excluido)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    SKU = p.SKU,
                    Price = p.Price,
                    Image = p.Image,
                    CreateTs = p.CreateTs,
                    ModTs = p.ModTs
                })
                .ToList();
        }

        public async Task<ProductDTO?> productGetById(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null || product.Excluido) return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                Image = product.Image,
                CreateTs = product.CreateTs,
                ModTs = product.ModTs
            };
        }

        public async Task<ProductDTO> productCreate(CreateProductDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Nome é obrigatório");
            if (dto.Price <= 0)
                throw new ArgumentException("Preço deve ser maior que zero");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                SKU = dto.SKU,
                Price = dto.Price,
                Image = dto.Image
            };

            var created = await _repository.Create(product);
            return new ProductDTO
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                SKU = created.SKU,
                Price = created.Price,
                Image = created.Image,
                CreateTs = created.CreateTs,
                ModTs = created.ModTs
            };
        }

        public async Task<ProductDTO> productUpdate(int id, UpdateProductDTO dto)
        {
            var product = await _repository.GetById(id);
            if (product == null || product.Excluido)
                throw new KeyNotFoundException($"Produto {id} não encontrado");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Nome é obrigatório");
            if (dto.Price <= 0)
                throw new ArgumentException("Preço deve ser maior que zero");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.SKU = dto.SKU;
            product.Price = dto.Price;
            product.Image = dto.Image;
            product.ModTs = DateTime.UtcNow;

            var updated = await _repository.Update(product);

            return new ProductDTO
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                SKU = updated.SKU,
                Price = updated.Price,
                Image = updated.Image,
                CreateTs = updated.CreateTs,
                ModTs = updated.ModTs
            };
        }

        public async Task<bool> productDelete(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null || product.Excluido) return false;

            //softdelete
            /*  product.Excluido = true;
              product.ModTs = DateTime.UtcNow;
              await _repository.Update(product);*/

            await _repository.Delete(id);
            return true;
        }
    }

}
