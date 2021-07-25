using System.Collections.Generic;

namespace InvoicePanel.Services.Product
{
    public interface IProductService
    {
        List<Data.Models.Product> GetAll();
        Data.Models.Product GetById(int id);
        ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product);
        ServiceResponse<Data.Models.Product> ArchiveProduct(int id);
    }
}