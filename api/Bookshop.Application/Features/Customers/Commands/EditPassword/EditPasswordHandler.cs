using AutoMapper;
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

namespace Bookshop.Application.Features.Customers.Commands.EditPassword
{
    public class EditPasswordHandler : ICommandHandler<EditPassword, CustomerCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public EditPasswordHandler(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<CustomerCommandResponse> Handle(EditPassword request, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerFromDto(request.Customer);
            await ChangeUserPassword(customer, request.Customer?.Password, request.Customer?.NewPassword);
            EditCustomerInDatabase(customer);
            await SaveChangesAsync(cancellationToken);
            var editedCustomerDto = _mapper.Map<CustomerResponseDto>(customer);
            var jwtSecurityToken = await JwtHelper.GenerateToken(_userManager, customer.IdentityData, _jwtSettings);
            return new()
            {
                Customer = editedCustomerDto,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer's password successfully updated",
                IsSaveChangesAsyncCalled = true
            };
        }
        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        private void EditCustomerInDatabase(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
        private async Task<Customer> GetCustomerFromDto(EditPasswordDto customerDto)
        {
            var customerExisting = await _dbContext.Customers
                                             .Include(x => x.BillingAddress)
                                             .Include(x => x.ShippingAddress)
                                             .Include(x => x.IdentityData)
                                             .FirstOrDefaultAsync(x => x.IdentityUserDataId == customerDto.UserId);
            return customerExisting;
        }

        private async Task ChangeUserPassword(Customer customer, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(customer.IdentityData, currentPassword, newPassword);

            if (!result.Succeeded)
                UserCreationExceptionHelper.ThrowUserCreationBadRequestException(result.Errors, customer?.IdentityData.UserName);
        }

        public Task ValidateRequest(EditPassword request)
        {
            throw new NotImplementedException();
        }
    }
}
