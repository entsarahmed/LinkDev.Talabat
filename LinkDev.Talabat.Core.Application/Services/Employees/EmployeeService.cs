using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Employees;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Employees;
using LinkDev.Talabat.Core.Domain.Specifications.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Employees
{
    internal class EmployeeService(IUnitOfWork unitOfWork, IMapper mapper) : IEmployeeService
    {
        public async Task<IEnumerable<EmployeesToReturnDto>> GetEmployeeAsync()
        {
            var spec = new EmployeeWithDepartmentSpecifications();
           var employees = await unitOfWork.GetRepository<Employee, int>().GetAllWithSpecAsync(spec);
            var employeesToReturn = mapper.Map<IEnumerable<EmployeesToReturnDto>>(employees);
            return employeesToReturn;
        }

        public async Task<EmployeesToReturnDto> GetEmployeeAsync(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);
            var employee = await unitOfWork.GetRepository<Employee, int>().GetAllWithSpecAsync(spec);
            var employeesToReturn = mapper.Map<EmployeesToReturnDto>(employee);
            return employeesToReturn;

        }
    }
}
