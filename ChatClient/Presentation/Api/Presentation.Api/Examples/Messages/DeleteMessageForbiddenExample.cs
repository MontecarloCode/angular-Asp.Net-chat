﻿using Core.Domain.Resources.Errors;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Api.Examples.Messages
{
    public class DeleteMessageForbiddenExample : IExamplesProvider<ErrorResource>
    {
        public ErrorResource GetExamples()
        {
            return new ErrorResource
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "Only the author of a message is allowed to delete a message"
            };
        }
    }
}
