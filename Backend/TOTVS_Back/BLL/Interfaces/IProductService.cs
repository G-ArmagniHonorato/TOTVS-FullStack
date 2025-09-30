
using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> productGetAll();
        Task<ProductDTO?> productGetById(int id);
        Task<ProductDTO> productCreate(CreateProductDTO dto);
        Task<ProductDTO> productUpdate(int id, UpdateProductDTO dto);
        Task<bool> productDelete(int id);
    }
}
