using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Base
{
    //[Route("api/products")]
    //[Route("api/buggy")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
