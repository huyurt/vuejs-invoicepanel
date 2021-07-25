using AutoMapper;
using InvoicePanel.Data.Models;
using InvoicePanel.Web.ViewModels;

namespace InvoicePanel.Web.Serialization
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<SalesOrder, InvoiceModel>().ReverseMap();
            CreateMap<SalesOrder, OrderModel>().ReverseMap();
            CreateMap<SalesOrderItem, SalesOrderItemModel>().ReverseMap();
            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<CustomerAddress, CustomerAddressModel>().ReverseMap();
            CreateMap<ProductInventory, ProductInventoryModel>().ReverseMap();
        }
    }
}