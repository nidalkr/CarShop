using CarShop.Domain.Entities.Appointments;
using CarShop.Domain.Entities.Catalog;
using CarShop.Domain.Entities.Commerce;
using CarShop.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities.Commerc
{
    public class OrderEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public CarShopUserEntity User { get; set; }
        public int CarId { get; set; }
        public CarEntity Car { get; set; }
        public int OrderStatusId { get; set; }
        public StatusEntity OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal FinalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public ICollection<DeliveryEntity> Deliveries { get; private set; } = new List<DeliveryEntity>();
        public ICollection<TransactionEntity> Transactions { get; private set; } = new List<TransactionEntity>();

    }
}
