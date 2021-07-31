using System.Collections.Generic;
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
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(IMapper mapper, ILogger<OrderController> logger, IOrderService orderService,
            ICustomerService customerService)
        {
            _mapper = mapper;
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Fatura oluşturuldu.");
            var order = _mapper.Map<SalesOrder>(invoice);
            order.Customer = _customerService.GetById(invoice.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok();
        }

        [HttpGet("/api/order")]
        public ActionResult GetOrders()
        {
            var orders = _orderService.GetAll();
            var orderModels = _mapper.Map<List<OrderModel>>(orders);
            return Ok(orderModels);
        }

        [HttpGet("/api/order/complete/{id}")]
        public ActionResult MakeOrderComplete(int id)
        {
            _logger.LogInformation($"{id} nolu sipariş tamamlanıyor...");
            _orderService.MarkFulfilled(id);
            return Ok();
        }
    }
}