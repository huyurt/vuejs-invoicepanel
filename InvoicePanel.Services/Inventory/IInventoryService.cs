using System.Collections.Generic;
using InvoicePanel.Data.Models;

namespace InvoicePanel.Services.Inventory
{
    public interface IInventoryService
    {
        List<ProductInventorySnapshot> GetSnapshotHistory();
        ProductInventory GetById(int id);
        void CreateSnapshot(ProductInventory inventory);
        List<ProductInventory> GetCurrentInventory();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
    }
}