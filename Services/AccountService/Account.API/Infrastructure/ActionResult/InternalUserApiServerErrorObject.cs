using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.Infrastructure.ActionResults
{
    public class InternalAccountApiServerErrorObject : ObjectResult
    {

        public InternalAccountApiServerErrorObject(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
