using MoneyManagement.Models.Salary;

namespace MoneyManagement.Interfaces
{
    public interface ISalaryService
    {
        Task<ICollection<Salary>> GetActiveSalaryList();
        Task<Salary> GetSalary(int salaryId);
        Task<Salary> AddSalary(Salary salary);
        Task<Salary> UpdateSalary(Salary salary);
        Task<Salary> DeleteSalary(Salary salary);

    }
}
