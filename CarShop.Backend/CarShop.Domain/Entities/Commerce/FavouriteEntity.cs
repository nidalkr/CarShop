using CarShop.Domain.Entities.Catalog;
using CarShop.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities.Commerce
{
    public class FavouriteEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CarShopUserEntity User { get; set; }
        public int CarId { get; set; }
        public CarEntity Car { get; set; }
        public DateTime DateAdded { get; set; }
        public string Notes { get; set; }
    }
}
