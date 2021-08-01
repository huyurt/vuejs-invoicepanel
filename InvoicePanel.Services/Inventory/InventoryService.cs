using System;
using System.Collections.Generic;
using System.Linq;
using InvoicePanel.Data;
using InvoicePanel.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoicePanel.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly InvoiceDbContext _context;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(InvoiceDbContext context, ILogger<InventoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
            return _context.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchived)
                .ToList();
        }

        public ProductInventory GetById(int id)
        {
            return _context.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product.Id == id);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _context.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _context.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Stok anlık kayıt oluşturulamadı.");
                    _logger.LogError(ex.StackTrace);
                }

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = _context.SaveChanges() > 1,
                    Data = inventory,
                    Message = $"{id} nolu ürünün stoğu güncellendi.",
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                };
            }
        }

        private void CreateSnapshot()
        {
            var now = DateTime.UtcNow;

            var inventories = _context.ProductInventories
                .Include(inv => inv.Product)
                .ToList();

            foreach (var inventory in inventories)
            {
                var snapshot = new ProductInventorySnapshot
                {
                    SnapshotTime = now,
                    Product = inventory.Product,
                    QuantityOnHand = inventory.QuantityOnHand
                };

                _context.Add(snapshot);
            }
        }
    }
}