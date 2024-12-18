using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Buggy
{
    public class BuggyController :BaseApiController
    {
        [HttpGet("notfound")] //GET: /api/buggy/notfound
         public IActionResult GetNotFoundFrror()
        {
            //throw new NotFoundException();
            // return NotFound(new {StatusCode = 404, Message = "Not Found "});//404
            return NotFound(new ApiResponse(404));
        }

        [HttpGet("servererror")] //GET: /api/buggy/servererror
        public IActionResult GetServerError()
        {
            throw new Exception(); // 500
        }
        [HttpGet("badrequest")]  //GET: /api/buggy/badrequest
        public IActionResult GetBadRequest()
        {
            // return BadRequest(); //400
            // return BadRequest( new {StatusCode=400, Message = "Bad Request"}); //400
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")] //GET: /api/buggy/badrequest/five
        public IActionResult GetValidationError(int id) //  => 400 
        {
           
            return Ok();
        }

        [HttpGet("unauthorized")] //GET: /api/buggy/unauthorized
        public IActionResult GetUnauthorizedError()
        {
            // return Unauthorized(); //401
            return Unauthorized(new ApiResponse(401)); //401 //401


        }

        [HttpGet("forbidden")] //GET: /api/buggy/forbidden
        public IActionResult GetForbiddenRequest()
        {
            return Forbid(); 
        }


        [Authorize]
        [HttpGet("authorized")]  //GET: /api/buggy/authorized
        public IActionResult GetAuthorizedRequest()
        {
            return Ok();
        }




    }
}
