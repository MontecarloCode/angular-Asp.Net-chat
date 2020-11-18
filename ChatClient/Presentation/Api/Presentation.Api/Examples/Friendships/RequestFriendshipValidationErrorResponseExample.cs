﻿using Core.Domain.Resources.Errors;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Presentation.Api.Examples.Friendships
{
    public class RequestFriendshipValidationErrorResponseExample : IExamplesProvider<ValidationErrorResource>
    {
        public ValidationErrorResource GetExamples()
        {
            return new ValidationErrorResource
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "One or multiple validation errors occurred",
                Errors = new Dictionary<string, IEnumerable<string>>
                {
                    { "AddresseeId", new [] { "'AddresseeId' must be greater than 0" } }
                }
            };
        }
    }
}