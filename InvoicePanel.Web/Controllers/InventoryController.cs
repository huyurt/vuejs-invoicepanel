using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using InvoicePanel.Services.Inventory;
using InvoicePanel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicePanel.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryService _inventoryService;

        public InventoryController(IMapper mapper, ILogger<InventoryController> logger,
            IInventoryService inventoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _inventoryService = inventoryService;
        }

        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory()
        {
            _logger.LogInformation("Tüm stoklar getiriliyor...");
            var inventories = _inventoryService.GetCurrentInventory();
            var inventoryModels = _mapper.Map<List<ProductInventoryModel>>(inventories)
                ?.OrderBy(inv => inv.Product.Name)
                ?.ToList();
            return Ok(inventoryModels);
        }

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"{shipment.ProductId} nolu ürünün stoğuna {shipment.Adjustment} ekleniyor...");
            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            var inventory = _inventoryService.UpdateUnitsAvailable(id, adjustment);
            return Ok(inventory);
        }

        [HttpGet("/api/inventory/snapshot")]
        public ActionResult GetSnapshotHistory()
        {
            _logger.LogInformation("Stok geçmişi getiriliyor...");

            try
            {
                var snapshotHistory = _inventoryService.GetSnapshotHistory();

                var timelineMarkers = snapshotHistory
                    .Select(t => t.SnapshotTime)
                    .Distinct()
                    .ToList();

                var snapshots = snapshotHistory
                    .GroupBy(hist => hist.Product, hist => hist.QuantityOnHand,
                        (key, g) => new ProductInventorySnapshotModel
                        {
                            ProductId = key.Id,
                            QuantityOnHand = g.ToList()
                        })
                    .OrderBy(hist => hist.ProductId)
                    .ToList();

                var viewModel = new SnapshotResponse
                {
                    Timeline = timelineMarkers,
                    ProductInventorySnapshots = snapshots
                };

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Stok geçmişi getirilirken hata oluştu.");
                _logger.LogError(ex.StackTrace);
                return BadRequest("Hata oluştu.");
            }
        }
    }
}