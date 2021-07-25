using System;
using System.Collections.Generic;
using System.Linq;
using InvoicePanel.Data;
using InvoicePanel.Data.Models;

namespace InvoicePanel.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly InvoiceDbContext _context;

        public ProductService(InvoiceDbContext context)
        {
            _context = context;
        }

        public List<Data.Models.Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Data.Models.Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _context.Products.Add(product);

                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10,
                };
                _context.ProductInventories.Add(newInventory);

                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = _context.SaveChanges() > 1,
                    Message = "Ürün oluşturuldu",
                    Data = product,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = false,
                    Data = product,
                    Message = ex.StackTrace,
                };
            }
        }

        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                product.IsArchived = true;

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Message = "Ürün arşivlendi",
                    IsSuccess = _context.SaveChanges() > 1,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                };
            }
        }
    }
}