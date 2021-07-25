using System;
using System.Collections.Generic;
using System.Linq;
using InvoicePanel.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoicePanel.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly InvoiceDbContext _context;

        public CustomerService(InvoiceDbContext context)
        {
            _context = context;
        }

        public List<Data.Models.Customer> GetAll()
        {
            return _context.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        public Data.Models.Customer GetById(int id)
        {
            return _context.Customers.Find(id);
        }

        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    Message = "Müşteri oluşturuldu",
                    IsSuccess = _context.SaveChanges() > 1,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    Message = ex.StackTrace,
                    IsSuccess = false,
                };
            }
        }

        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Müşteri bulunamadı",
                    IsSuccess = false,
                };
            }

            try
            {
                _context.Customers.Remove(customer);
                return new ServiceResponse<bool>
                {
                    Data = _context.SaveChanges() > 1,
                    Message = "Müşteri silindi.",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = ex.StackTrace,
                    IsSuccess = false,
                };
            }
        }
    }
}