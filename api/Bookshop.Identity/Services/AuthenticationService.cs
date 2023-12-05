using AutoMapper;
using Bookshop.Application.Contracts.Identity;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Customer;
using Bookshop.Application.Features.Customer.Commands;
using Bookshop.Application.Features.Customer.Queries.Authenticate;
using Bookshop.Domain.Entities;
using Bookshop.Identity.JwtModel;
using Bookshop.Identity.Roles;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public AuthenticationService(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<AuthenticateQueryResponse> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new NotFoundException(nameof(username), username);
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new ValidationException($"Credentials for '{username} aren't valid'.");
            }
            var userRoles = await _userManager.GetRolesAsync(user); // Get the roles of the user
            JwtSecurityToken? jwtSecurityToken = await GenerateToken(user, userRoles);
            var customer = _mapper.Map<CustomerDto>(_dbContext.Customers.Include(x => x.IdentityData).FirstOrDefault(x => x.IdentityUserDataId == user.Id));
            return new AuthenticateQueryResponse
            {
                Customer = customer,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken)
            };
        }

        public async Task<CreateCustomerCommandResponse> CreateCustomer(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Input validation
            if (request.Customer == null)
                throw new BadRequestException($"{nameof(request.Customer)}, Customer information is required");
            

            var newUser = CreateNewCustomer(request.Customer);

            await CreateUserAndRole(newUser, request.Customer?.Password);

            var user = await _userManager.FindByNameAsync(newUser?.IdentityData.UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken? jwtSecurityToken = await GenerateToken(user, userRoles);

            await _dbContext.Customers.AddAsync(newUser, cancellationToken);

            return new CreateCustomerCommandResponse
            {
                Id = user.Id,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer {request.Customer?.FirstName} successfully created"
            };
        }

        private Customer CreateNewCustomer(CustomerDto customerDto)
        {
            var shippingAddress = new Address(customerDto?.ShippingAddress.Street, customerDto?.ShippingAddress.City, customerDto?.ShippingAddress.PostalCode, customerDto?.ShippingAddress.Country, customerDto?.ShippingAddress.State);
            var billingAddress = new Address(customerDto?.BillingAddress.Street, customerDto?.BillingAddress.City, customerDto?.BillingAddress.PostalCode, customerDto?.BillingAddress.Country, customerDto?.BillingAddress.State);

            return new Customer(customerDto.FirstName, customerDto.LastName, shippingAddress, billingAddress)
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
            {
                var errors = resultUser.Errors.ToList().Select(x => x.Description).ToList();
                throw new BadRequestException($"Failed to create user {newUser?.IdentityData.UserName}", errors);
            }

            var resultRole = await _userManager.AddToRoleAsync(newUser.IdentityData, RoleNames.Customer);
            if (!resultRole.Succeeded)
            {
                var errors = resultRole.Errors.ToList().Select(x => x.Description).ToList();
                throw new BadRequestException($"Failed to create user's role {newUser?.IdentityData.UserName}", errors);
            }
        }


        private async Task<JwtSecurityToken?> GenerateToken(IdentityUserData user, IList<string> userRoles)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            }
            .Union(userClaims)
            .Union(userRoles.Select(role => new Claim("role", role))); // Add the roles as claims

            if (_jwtSettings.Key != null)
            {
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                // Adjust token expiration as needed (e.g., 1 hour)
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10), // Change to your desired expiration time
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }

            return null;
        }
    }
}
