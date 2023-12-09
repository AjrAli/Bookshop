﻿using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerResponse : BaseResponse
    {
        public CreateCustomerResponse() : base()
        {

        }
        public CustomerResponseDto? Customer { get; set; }
        public string? Token { get; set; }
    }
}
