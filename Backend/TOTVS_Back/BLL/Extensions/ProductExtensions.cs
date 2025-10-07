using BLL.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDTO ToProductDTO(this Product product)
        {
            if (product == null) return null;

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

        public static List<ProductDTO> ToDTOList(this IEnumerable<Product> products)
        {
            return products?.Select(p => p.ToProductDTO()).ToList() ?? new List<ProductDTO>();
        }
        public static Product ToEntity(this CreateProductDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Nome é obrigatório");
            if (dto.Price <= 0)
                throw new ArgumentException("Preço deve ser maior que zero");

            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                SKU = dto.SKU,
                Price = dto.Price,
                Image = dto.Image
            };
        }

        public static Product ApplyUpdate(this Product product, UpdateProductDTO dto)
        {
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

            return product;
        }
    }
}
