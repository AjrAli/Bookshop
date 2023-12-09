﻿using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customers.Queries.Authenticate
{
    public class AuthenticateResponse : BaseResponse
    {
        public AuthenticateResponse() : base()
        {

        }
        public CustomerRequestDto? Customer { get; set; }
        public string? Token { get; set; }
    }
}
