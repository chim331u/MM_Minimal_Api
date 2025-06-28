using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Contract
{
    public class Bank_DTO
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; } //bank name
        public string? Description { get; set; }
        public string? WebUrl { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public string? ReferenceName { get; set; }

        public int CountryId { get; set; }
        public string? CountryName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string? Note { get; set; }

    }
}
