using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using InvoicePanel.Data.Models;
using InvoicePanel.Services.Customer;
using InvoicePanel.Services.Order;
using InvoicePanel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicePanel.Web.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(IMapper mapper, ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            _logger.LogInformation("Yeni müşteri oluşturuluyor...");
            customer.CreatedOn = DateTime.UtcNow;
            customer.UpdatedOn = DateTime.UtcNow;
            var customerData = _mapper.Map<Customer>(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);
            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomers()
        {
            _logger.LogInformation("Müşteriler getiriliyor...");
            var customers = _customerService.GetAll();
            var customerModels = _mapper.Map<List<CustomerModel>>(customers)
                .OrderByDescending(customer => customer.CreatedOn)
                .ToList();
            return Ok(customerModels);
        }

        [HttpDelete("/api/customer/{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation("Müşteri siliniyor...");
            var response = _customerService.DeleteCustomer(id);
            return Ok(response);
        }
    }
}