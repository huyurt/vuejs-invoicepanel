using System.Collections.Generic;
using InvoicePanel.Data.Models;

namespace InvoicePanel.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetAll();
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
        ServiceResponse<bool> MarkFulfilled(int id);
    }
}