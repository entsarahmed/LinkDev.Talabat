using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LinkDev.Talabat.Core.Application.Exceptions
{
    // public class BadRequestException : ApplicationException
    //{
    //    public required IEnumerable<string> Errors { get; set; }
    //    public BadRequestException(string? message = "Bad Request", IEnumerable<string>? errors = null)
    //        : base(message) 
    //    {
    //        Errors = errors ?? Enumerable.Empty<string>();

    //    }
    //}
    public class BadRequestException : ApplicationException
    {
        public  IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public BadRequestException(string? message = "Bad Request", IEnumerable<string>? errors = null)
            : base(message)
        {
            if (errors != null)
                Errors = errors;
        }
    }

}
