using CarShop.Domain.Common;
using CarShop.Domain.Entities.Appointments;
using CarShop.Domain.Entities.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CarShop.Domain.Entities.Commerce;

public sealed class StatusEntity:BaseEntity
{
    public string StatusName { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<OrderEntity> Orders { get; private set; } = new List<OrderEntity>();
    public ICollection<TransactionEntity> Transactions { get; private set; } = new List<TransactionEntity>();
    public ICollection<TestDriveEntity> TestDrives { get; private set; } = new List<TestDriveEntity>();
    public ICollection<ServiceAppointmentEntity> ServiceAppointments { get; private set; } = new List<ServiceAppointmentEntity>();
    public ICollection<DeliveryEntity> Deliveries { get; private set; } = new List<DeliveryEntity>();
    public ICollection<InquiryEntity> Inquiries { get; private set; } = new List<InquiryEntity>();

}
