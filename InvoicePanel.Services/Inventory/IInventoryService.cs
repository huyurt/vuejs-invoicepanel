using System.Collections.Generic;
using InvoicePanel.Data.Models;

namespace InvoicePanel.Services.Inventory
{
    public interface IInventoryService
    {
        List<ProductInventorySnapshot> GetSnapshotHistory();
        ProductInventory GetById(int id);
        List<ProductInventory> GetCurrentInventory();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
    }
}