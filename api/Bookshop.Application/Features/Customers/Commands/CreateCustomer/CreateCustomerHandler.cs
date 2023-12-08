using AutoMapper;
using Bookshop.Application.Configuration;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Helpers;
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
            await ValidateRequest(request);
            var newUser = CreateNewCustomerFromDto(request.Customer);
            await CreateUserAndRole(newUser, request.Customer?.Password);
            var user = await _userManager.FindByNameAsync(newUser?.IdentityData.UserName);
            var jwtSecurityToken = GenerateJwtToken(user);
            await StoreCustomerInDatabase(request, newUser, cancellationToken);
            var customerCreated = _mapper.Map<CustomerDto>(_dbContext.Customers.Include(x => x.IdentityData).FirstOrDefault(x => x.IdentityUserDataId == user.Id));
            return new()
            {
                Customer = customerCreated,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer {customerCreated.FirstName} successfully created"
            };
        }

        private async Task StoreCustomerInDatabase(CreateCustomer request, Customer customer, CancellationToken cancellationToken)
        {
            await _dbContext.Customers.AddAsync(customer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        public Task ValidateRequest(CreateCustomer request)
        {
            _ = request.Customer ?? throw new ValidationException($"{nameof(request.Customer)}, Customer information is required");
            return Task.CompletedTask;
        }

        private JwtSecurityToken GenerateJwtToken(IdentityUserData user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var userClaims = _userManager.GetClaimsAsync(user).Result;
            return JwtHelper.GenerateToken(user, userClaims, userRoles, _jwtSettings);
        }

        private Customer CreateNewCustomerFromDto(CustomerDto customerDto)
        {
            var shippingAddress = new Address(
                customerDto?.ShippingAddress.Street,
                customerDto?.ShippingAddress.City,
                customerDto?.ShippingAddress.PostalCode,
                customerDto?.ShippingAddress.Country,
                customerDto?.ShippingAddress.State
            );

            var billingAddress = new Address(
                customerDto?.BillingAddress.Street,
                customerDto?.BillingAddress.City,
                customerDto?.BillingAddress.PostalCode,
                customerDto?.BillingAddress.Country,
                customerDto?.BillingAddress.State
            );

            return new Customer(
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

        private async Task CreateUserAndRole(Customer newUser, string password)
        {
            var resultUser = await _userManager.CreateAsync(newUser.IdentityData, password);

            if (!resultUser.Succeeded)
                ThrowBadRequestException(resultUser.Errors, newUser?.IdentityData.UserName);

            var resultRole = await _userManager.AddToRoleAsync(newUser.IdentityData, RoleNames.Customer);

            if (!resultRole.Succeeded)
                ThrowBadRequestException(resultRole.Errors, newUser?.IdentityData.UserName);
        }

        private void ThrowBadRequestException(IEnumerable<IdentityError> errors, string userName)
        {
            var errorDescriptions = errors.Select(x => x.Description).ToList();
            throw new BadRequestException($"Failed to create user {userName}", errorDescriptions);
        }
    }
}
