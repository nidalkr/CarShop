using CarShop.Domain.Entities.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities.Appointments
{
    public class DeliveryEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
        public int StatusId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string Address { get; set; }
        public int TrackingNumber { get; set; }
        public StatusEntity Status { get; set; }
    }
}
