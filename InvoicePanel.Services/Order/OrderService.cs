using System;
using System.Collections.Generic;
using System.Linq;
using InvoicePanel.Data;
using InvoicePanel.Data.Models;
using InvoicePanel.Services.Inventory;
using InvoicePanel.Services.Product;
using Microsoft.EntityFrameworkCore;

namespace InvoicePanel.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly InvoiceDbContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;

        public OrderService(InvoiceDbContext context, IInventoryService inventoryService, IProductService productService)
        {
            _context = context;
            _inventoryService = inventoryService;
            _productService = productService;
        }

        public List<SalesOrder> GetAll()
        {
            return _context.SalesOrders
                .Include(so => so.Customer)
                .ThenInclude(customer => customer.PrimaryAddress)
                .Include(so => so.SalesOrderItems)
                .ThenInclude(item => item.Product)
                .ToList();
        }

        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetById(item.Product.Id);

                var inventoryId = _inventoryService.GetById(item.Product.Id).Id;

                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            try
            {
                _context.SalesOrders.Add(order);

                return new ServiceResponse<bool>
                {
                    Data = _context.SaveChanges() > 1,
                    Message = "Sipariş oluşturuldu.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = ex.StackTrace,
                };
            }
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var now = DateTime.UtcNow;
            var order = _context.SalesOrders.Find(id);
            order.UpdatedOn = now;
            order.IsPaid = true;

            try
            {
                _context.SalesOrders.Update(order);

                return new ServiceResponse<bool>
                {
                    Data = _context.SaveChanges() > 1,
                    Message = $"{id} nolu siparişin ödemesi yapıldı.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = ex.StackTrace,
                };
            }
        }
    }
}