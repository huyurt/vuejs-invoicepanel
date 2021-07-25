using System.Collections.Generic;

namespace InvoicePanel.Services.Customer
{
    public interface ICustomerService
    {
        List<Data.Models.Customer> GetAll();
        Data.Models.Customer GetById(int id);
        ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer);
        ServiceResponse<bool> DeleteCustomer(int id);
    }
}