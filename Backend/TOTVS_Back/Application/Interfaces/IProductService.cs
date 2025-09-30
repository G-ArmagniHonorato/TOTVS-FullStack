using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAll();
        Task<ProductDTO?> GetById(int id);
        Task<ProductDTO> Create(CreateProductDTO dto);
        Task<ProductDTO> Update(int id, UpdateProductDTO dto);
        Task<bool> Delete(int id);
    }
}
