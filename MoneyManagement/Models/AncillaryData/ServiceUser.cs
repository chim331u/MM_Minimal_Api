using MoneyManagement.Models.Utility;

namespace MoneyManagement.Models.AncillaryData
{
    public class ServiceUser : BaseEntity
    {

        public int Id { get; set; }


        public string? Name { get; set; }
        public string? Surname { get; set; }

    }
}