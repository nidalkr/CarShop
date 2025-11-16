using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities.Appointments
{
    public class ServiceRecordEntity
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public ServiceAppointmentEntity Appointment { get; set; }
        public DateTime ServiceDate { get; set; }
        public int MileageAtService { get; set; }
        public string WorkDescription { get; set; }
        public string PartsUsed { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalCost { get; set; }

    }
}
