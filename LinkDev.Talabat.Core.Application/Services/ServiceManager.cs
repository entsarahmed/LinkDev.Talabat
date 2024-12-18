using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Employees;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Application.Services.Employees;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using Microsoft.Extensions.Configuration;

namespace LinkDev.Talabat.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthService> _authService;
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, Func<IAuthService> authServiceFactory, Func<IOrderService> orderServiceFactor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration=configuration;
            _orderService = new Lazy<IOrderService>(orderServiceFactor, LazyThreadSafetyMode.ExecutionAndPublication);
            _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(_unitOfWork, _mapper));
            _authService = new Lazy<IAuthService>(authServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IProductService ProductService => _productService.Value; //Return Backing Field.Value
        public IEmployeeService EmployeeService => _employeeService.Value; //Return Backing Field.Value


        public IAuthService AuthService => _authService.Value;

        public IOrderService OrderService => _orderService.Value;
    }
}
