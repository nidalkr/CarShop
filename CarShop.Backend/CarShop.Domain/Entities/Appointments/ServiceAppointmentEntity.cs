using CarShop.Domain.Entities.Catalog;
using CarShop.Domain.Entities.Commerc;
using CarShop.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities.Appointments
{
    public class ServiceAppointmentEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public CarEntity Car { get; set; }
        public int UserId { get; set; }
        public CarShopUserEntity User { get; set; }
        //Missing Mechanic Entity
        public string ServiceType { get; set; }
        public string? CustomerNotes { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public int StatusId { get; set; }
        public StatusEntity Status { get; set; }

        public List<ServiceRecordEntity> ServiceRecords { get; set; } = new();
    }
}
