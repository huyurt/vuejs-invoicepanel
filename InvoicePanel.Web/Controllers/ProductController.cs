using System.Collections.Generic;
using AutoMapper;
using InvoicePanel.Data.Models;
using InvoicePanel.Services.Product;
using InvoicePanel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicePanel.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, ILogger<ProductController> logger, IProductService productService)
        {
            _mapper = mapper;
            _logger = logger;
            _productService = productService;
        }

        [HttpPost("/api/product")]
        public ActionResult AddProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Ürün oluşturuluyor...");
            var newProduct = _mapper.Map<Product>(product);
            var newProductResponse = _productService.CreateProduct(newProduct);
            return Ok(newProductResponse);
        }

        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Tüm ütünler getiriliyor...");
            var products = _productService.GetAll();
            return Ok(_mapper.Map<List<ProductModel>>(products));
        }

        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Ürün arşivlendi...");
            var archiveResult = _productService.ArchiveProduct(id);
            return Ok(archiveResult);
        }
    }
}