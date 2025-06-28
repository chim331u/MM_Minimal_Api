using System.ComponentModel.DataAnnotations;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.AncillaryData
{
    public class Supplier : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? UnitMeasure { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Contract { get; set; }

        //other details
    }
}
