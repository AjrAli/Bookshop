using AutoMapper;
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

namespace Bookshop.Application.Features.Customers.Commands.EditCustomer
{
    public class EditCustomerHandler : ICommandHandler<EditCustomer, EditCustomerResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public EditCustomerHandler(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<EditCustomerResponse> Handle(EditCustomer request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            var editedCustomer = await EditCustomerFromDto(request.Customer);
            await ChangeUserPassword(editedCustomer, request.Customer?.Password, request.Customer?.NewPassword);
            EditCustomerInDatabase(editedCustomer);
            await SaveChangesAsync(request, cancellationToken);
            var editedCustomerDto = await _dbContext.Customers.Include(x => x.IdentityData)
                                   .Where(x => x.Id == editedCustomer.Id)
                                   .Select(x => _mapper.Map<CustomerResponseDto>(x))
                                   .FirstOrDefaultAsync();
            var jwtSecurityToken = GenerateJwtToken(editedCustomer.IdentityData);
            return new()
            {
                Customer = editedCustomerDto,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer {editedCustomerDto.FirstName} successfully edited"
            };
        }
        private async Task SaveChangesAsync(EditCustomer request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private void EditCustomerInDatabase(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
        public async Task ValidateRequest(EditCustomer request)
        {
            _ = request.Customer ?? throw new ValidationException($"{nameof(request.Customer)}, Customer information is required");
            if (request.Customer.Id == 0 || !await _dbContext.Customers.Include(x => x.IdentityData)
                                                                       .AnyAsync(x => x.Id == request.Customer.Id &&
                                                                                      x.IdentityUserDataId == request.Customer.IdentityUserDataId))
                throw new ValidationException($"Customer {request.Customer.Id} doesn't exist");
        }

        private JwtSecurityToken GenerateJwtToken(IdentityUserData user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var userClaims = _userManager.GetClaimsAsync(user).Result;
            return JwtHelper.GenerateToken(user, userClaims, userRoles, _jwtSettings);
        }

        private async Task<Customer> EditCustomerFromDto(EditCustomerDto customerDto)
        {
            var customerExisting = await _dbContext.Customers
                                             .Include(x => x.BillingAddress)
                                             .Include(x => x.ShippingAddress)
                                             .Include(x => x.IdentityData)
                                             .FirstOrDefaultAsync(x => x.Id == customerDto.Id && x.IdentityUserDataId == customerDto.IdentityUserDataId);
            customerExisting.BillingAddress.EditAdress(_mapper.Map<Address>(customerDto.BillingAddress));
            customerExisting.ShippingAddress.EditAdress(_mapper.Map<Address>(customerDto.ShippingAddress));
            customerExisting.FirstName = customerDto.FirstName;
            customerExisting.LastName = customerDto.LastName;
            return customerExisting;
        }

        private async Task ChangeUserPassword(Customer customer, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(customer.IdentityData, currentPassword, newPassword);

            if (!result.Succeeded)
                ThrowBadRequestException(result.Errors, customer?.IdentityData.UserName);
        }

        private void ThrowBadRequestException(IEnumerable<IdentityError> errors, string userName)
        {
            var errorDescriptions = errors.Select(x => x.Description).ToList();
            throw new BadRequestException($"Failed to edit user {userName}", errorDescriptions);
        }
    }
}
