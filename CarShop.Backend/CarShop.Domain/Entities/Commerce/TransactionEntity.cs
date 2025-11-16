using CarShop.Domain.Entities.Commerc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities.Commerce
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int StatusId { get; set; }
        public StatusEntity Status { get; set; }
        public string? FinancingProvider { get; set; }
        public decimal? InterestRate { get; set; }
        public int? LoanTermMonths { get; set; }


    }
}
