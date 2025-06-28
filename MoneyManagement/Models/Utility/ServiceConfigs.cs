using System.ComponentModel.DataAnnotations;

namespace MoneyManagement.Models.Utility
{
    public class ServiceConfigs
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
