using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Exceptions
{
    
        public class ValidationException : BadRequestException
        {
            // No need to redeclare 'Errors', it's inherited from BadRequestException

            public ValidationException(string? message = "Validation Error", IEnumerable<string>? errors = null)
                : base(message, errors)
            {
            }
        }
    

}
