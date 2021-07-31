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
    }
}