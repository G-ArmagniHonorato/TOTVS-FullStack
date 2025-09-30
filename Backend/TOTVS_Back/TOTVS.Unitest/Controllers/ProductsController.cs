using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Domain.Entities;
using BLL.DTOs;
using BLL.Interfaces;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _serviceMock = new Mock<IProductService>();
        _controller = new ProductsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        var products = new List<ProductDTO>
        {
            new ProductDTO { Id = 1, Name = "Prego", Price = 10 },
            new ProductDTO { Id = 2, Name = "Parafuso", Price = 20 }
        };

        _serviceMock.Setup(productService => productService.productGetAll()).ReturnsAsync(products);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProducts = Assert.IsAssignableFrom<List<ProductDTO>>(okResult.Value);
        Assert.Equal(2, returnProducts.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkWithProduct()
    {
        var product = new ProductDTO { Id = 1, Name = "Prego", Price = 10 };

        _serviceMock.Setup(productService => productService.productGetById(1)).ReturnsAsync(product);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProduct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Equal(1, returnProduct.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        _serviceMock.Setup(productService => productService.productGetById(99)).ReturnsAsync((ProductDTO)null);

        var result = await _controller.GetById(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsOkWithProduct()
    {
        var createDto = new CreateProductDTO
        {
            Name = "Prego",
            Description = "Prego desc",
            SKU = "9999990",
            Price = 100
        };

        var createdProduct = new ProductDTO
        {
            Id = 1,
            Name = "Prego",
            Description = "Prego desc",
            SKU = "9999990",
            Price = 100,
            CreateTs = DateTime.Now,
            ModTs = DateTime.Now
        };

        _serviceMock.Setup(productService => productService.productCreate(createDto))
                    .ReturnsAsync(createdProduct);

        var result = await _controller.Create(createDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProduct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Equal("Prego", returnProduct.Name);
        Assert.Equal("Prego desc", returnProduct.Description);
        Assert.Equal("9999990", returnProduct.SKU);
        Assert.Equal(100, returnProduct.Price);
    }

    [Fact]
    public async Task Update_ReturnsOkWithUpdatedProduct()
    {
        var updateDto = new UpdateProductDTO { Name = "Prego atualizado", Price = 122 };
        var updatedProduct = new ProductDTO { Id = 1, Name = "Produto Atualizado", Price = 15 };

        _serviceMock.Setup(productService => productService.productUpdate(1, updateDto)).ReturnsAsync(updatedProduct);

        var result = await _controller.Update(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProduct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Equal("Produto Atualizado", returnProduct.Name);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var updateDto = new UpdateProductDTO { Name = "Parafuso", Price = 50 };

        _serviceMock.Setup(productService => productService.productUpdate(99, updateDto)).ReturnsAsync((ProductDTO)null);

        var result = await _controller.Update(99, updateDto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        _serviceMock.Setup(productService => productService.productDelete(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
    {
        _serviceMock.Setup(productService => productService.productDelete(99)).ReturnsAsync(false);

        var result = await _controller.Delete(99);

        Assert.IsType<NotFoundResult>(result);
    }
}
