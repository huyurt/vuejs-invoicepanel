using System;
using System.Collections.Generic;

namespace InvoicePanel.Data.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsPaid { get; set; }
        
        public Customer Customer { get; set; }
        public List<SalesOrderItem> SalesOrderItems { get; set; }
    }
}