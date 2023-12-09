using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
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
            var jwtSecurityToken = JwtHelper.GenerateToken(_userManager, editedCustomer.IdentityData, _jwtSettings);
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
        }
        private async Task<Customer> EditCustomerFromDto(EditCustomerDto customerDto)
        {
            var customerExisting = await _dbContext.Customers
                                             .Include(x => x.BillingAddress)
                                             .Include(x => x.ShippingAddress)
                                             .Include(x => x.IdentityData)
                                             .FirstOrDefaultAsync(x => x.IdentityUserDataId == customerDto.UserId);
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
                UserCreationExceptionHelper.ThrowUserCreationBadRequestException(result.Errors, customer?.IdentityData.UserName);
        }

    }
}
