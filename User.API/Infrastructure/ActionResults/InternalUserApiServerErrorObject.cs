using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Infrastructure.ActionResults
{
    public class InternalUserApiServerErrorObject : ObjectResult
    {

        public InternalUserApiServerErrorObject(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
