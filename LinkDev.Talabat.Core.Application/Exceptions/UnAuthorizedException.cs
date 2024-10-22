using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Exceptions
{
    public class UnAuthorizedException : ApplicationException
    {
        public required IEnumerable<string> Errors { get; set; }

        public UnAuthorizedException(string? message = "UnAuthorized Request", IEnumerable<string>? errors = null) : base(message) 
        {
            
        }
    }
}
