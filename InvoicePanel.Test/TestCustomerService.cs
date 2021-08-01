using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InvoicePanel.Data;
using InvoicePanel.Data.Models;
using InvoicePanel.Services.Customer;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InvoicePanel.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void CustomerService_GetsAllCustomers_GivenTheyExist()
        {
            var options = new DbContextOptionsBuilder<InvoiceDbContext>()
                .UseInMemoryDatabase("gets_all").Options;

            using var context = new InvoiceDbContext(options);

            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 123123});
            sut.CreateCustomer(new Customer {Id = -213});

            var allCustomers = sut.GetAll();

            allCustomers.Count.Should().Be(2);
        }

        [Fact]
        public void CustomerService_CreatesCustomer_GivenNewCustomerObject()
        {
            var options = new DbContextOptionsBuilder<InvoiceDbContext>()
                .UseInMemoryDatabase("Add_writes_to_database").Options;

            using var context = new InvoiceDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 18645});
            context.Customers.Single().Id.Should().Be(18645);
        }

        [Fact]
        public void CustomerService_DeletesCustomer_GivenId()
        {
            var options = new DbContextOptionsBuilder<InvoiceDbContext>()
                .UseInMemoryDatabase("deletes_one")
                .Options;

            using var context = new InvoiceDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer {Id = 18645});

            sut.DeleteCustomer(18645);
            var allCustomers = sut.GetAll();
            allCustomers.Count.Should().Be(0);
        }

        [Fact]
        public void CustomerService_OrdersByLastName_WhenGetAllCustomersInvoked()
        {
            // Arrange
            var data = new List<Customer>
            {
                new Customer {Id = 123, LastName = "Hudayfe"},
                new Customer {Id = 323, LastName = "Ahmet"},
                new Customer {Id = -89, LastName = "Mehmet"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mockContext = new Mock<InvoiceDbContext>();

            mockContext.Setup(c => c.Customers)
                .Returns(mockSet.Object);

            // Act
            var sut = new CustomerService(mockContext.Object);
            var customers = sut.GetAll();

            // Assert
            customers.Count.Should().Be(3);
            customers[0].Id.Should().Be(323);
            customers[1].Id.Should().Be(-89);
            customers[2].Id.Should().Be(123);
        }
    }
}