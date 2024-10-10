using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services.Employees
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeesToReturnDto>> GetEmployeeAsync();
        Task<EmployeesToReturnDto> GetEmployeeAsync(int id);
    }
}
