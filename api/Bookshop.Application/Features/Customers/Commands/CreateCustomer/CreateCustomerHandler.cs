using AutoMapper;
using Bookshop.Application.Configuration;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Application.Features.Customers.Helpers;
using Bookshop.Application.Settings;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerHandler : ICommandHandler<CreateCustomer, CreateCustomerResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public CreateCustomerHandler(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomer request, CancellationToken cancellationToken)
        {
            var newCustomer = await CreateNewCustomerFromDto(request.Customer);
            await CreateUserAndRole(newCustomer, request.Customer?.Password);
            var user = await _userManager.FindByNameAsync(newCustomer?.IdentityData.UserName);
            var jwtSecurityToken = await JwtHelper.GenerateToken(_userManager, user, _jwtSettings);
            await StoreCustomerInDatabase(request, newCustomer, cancellationToken);
            var customerCreated = await _dbContext.Customers.Include(x => x.IdentityData)
                                   .Where(x => x.IdentityUserDataId == user.Id)
                                   .Select(x => _mapper.Map<CustomerResponseDto>(x))
                                   .FirstOrDefaultAsync();
            return new()
            {
                Customer = customerCreated,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer {customerCreated.FirstName} successfully created",
                IsSaveChangesAsyncCalled = true
            };
        }

        private async Task StoreCustomerInDatabase(CreateCustomer request, Customer customer, CancellationToken cancellationToken)
        {
            await _dbContext.Customers.AddAsync(customer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<Customer> CreateNewCustomerFromDto(CustomerRequestDto customerDto)
        {

            var shippingAddress = new Address(
                customerDto?.ShippingAddress.Street,
                customerDto?.ShippingAddress.City,
                customerDto?.ShippingAddress.PostalCode,
                customerDto?.ShippingAddress.Country,
                customerDto?.ShippingAddress.State
            )
            {
                LocationPricing = await _dbContext.FindLocationPricingByCountry(customerDto?.ShippingAddress.Country)
            };

            var billingAddress = new Address(
                customerDto?.BillingAddress.Street,
                customerDto?.BillingAddress.City,
                customerDto?.BillingAddress.PostalCode,
                customerDto?.BillingAddress.Country,
                customerDto?.BillingAddress.State
            );

            return new(
                customerDto.FirstName,
                customerDto.LastName,
                shippingAddress,
                billingAddress
            )
            {
                IdentityData = new IdentityUserData
                {
                    UserName = customerDto.Username,
                    Email = customerDto.Email,
                    EmailConfirmed = customerDto.EmailConfirmed
                }
            };
        }

        private async Task CreateUserAndRole(Customer customer, string password)
        {
            var resultUser = await _userManager.CreateAsync(customer.IdentityData, password);

            if (!resultUser.Succeeded)
                UserCreationExceptionHelper.ThrowUserCreationBadRequestException(resultUser.Errors, customer?.IdentityData.UserName);

            var resultRole = await _userManager.AddToRoleAsync(customer.IdentityData, RoleNames.Customer);

            if (!resultRole.Succeeded)
                UserCreationExceptionHelper.ThrowUserCreationBadRequestException(resultRole.Errors, customer?.IdentityData.UserName);
        }

        public Task ValidateRequest(CreateCustomer request)
        {
            throw new NotImplementedException();
        }
    }
}
