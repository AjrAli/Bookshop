using AutoMapper;
using Bookshop.Application.Configuration;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Application.Settings;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Application.Features.Customer.Commands
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var newUser = CreateNewCustomerFromDto(request.Customer);
            await CreateUserAndRole(newUser, request.Customer?.Password);

            var user = await _userManager.FindByNameAsync(newUser?.IdentityData.UserName);
            var jwtSecurityToken = GenerateJwtToken(user);

            await _dbContext.Customers.AddAsync(newUser, cancellationToken);

            return CreateResponse(user, jwtSecurityToken, request.Customer?.FirstName);
        }

        private void ValidateRequest(CreateCustomerCommand request)
        {
            if (request.Customer == null)
                throw new ValidationException($"{nameof(request.Customer)}, Customer information is required");
        }

        private JwtSecurityToken GenerateJwtToken(IdentityUserData user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var userClaims = _userManager.GetClaimsAsync(user).Result;
            return JwtHelper.GenerateToken(user, userClaims, userRoles, _jwtSettings);
        }

        private CreateCustomerCommandResponse CreateResponse(IdentityUserData user, JwtSecurityToken jwtSecurityToken, string firstName)
        {
            return new CreateCustomerCommandResponse
            {
                Id = user.Id,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer {firstName} successfully created"
            };
        }

        private Domain.Entities.Customer CreateNewCustomerFromDto(CustomerDto customerDto)
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

            return new Domain.Entities.Customer(
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

        private async Task CreateUserAndRole(Domain.Entities.Customer newUser, string password)
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
