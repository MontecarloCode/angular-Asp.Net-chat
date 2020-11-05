﻿using Core.Domain.Dtos.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Api.Examples.Users
{
    public class LoginRequestExample : IExamplesProvider<LoginCredentialsDto>
    {
        public LoginCredentialsDto GetExamples()
        {
            return new LoginCredentialsDto
            {
                UserNameOrEmail = "alfred.miller@gmail.com",
                Password = "p455w0rd"
            };
        }
    }
}
