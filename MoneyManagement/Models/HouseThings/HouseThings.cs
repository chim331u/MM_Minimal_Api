using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.HouseThings
{
    public class HouseThings : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ItemType { get; set; }
        public string? Model { get; set; }
        public double Cost { get; set; }
        public int HistoryId { get; set; }
        public DateTime PurchaseDate { get; set; }

        public HouseThingsRooms Room { get; set; }

    }
}
