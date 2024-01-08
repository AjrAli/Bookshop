using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Application.Settings;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Application.Features.Customers.Commands.EditProfile
{
    public class EditProfileHandler : ICommandHandler<EditProfile, CustomerCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public EditProfileHandler(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<CustomerCommandResponse> Handle(EditProfile request, CancellationToken cancellationToken)
        {
            var editedCustomer = await EditCustomerProfileFromDto(request.Customer);
            EditCustomerInDatabase(editedCustomer);
            await SaveChangesAsync(cancellationToken);
            var editedCustomerDto = await _dbContext.Customers.Include(x => x.BillingAddress)
                                                              .Include(x => x.ShippingAddress)
                                                              .Include(x => x.IdentityData)
                                                              .Where(x => x.Id == editedCustomer.Id)
                                                              .Select(x => _mapper.Map<CustomerResponseDto>(x))
                                                              .FirstOrDefaultAsync();
            var jwtSecurityToken = await JwtHelper.GenerateToken(_userManager, editedCustomer.IdentityData, _jwtSettings);
            return new()
            {
                Customer = editedCustomerDto,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Message = $"Customer's profile successfully updated",
                IsSaveChangesAsyncCalled = true
            };
        }
        private async Task<Customer> EditCustomerProfileFromDto(EditProfileDto customerDto)
        {
            var customerExisting = await _dbContext.Customers
                                             .Include(x => x.BillingAddress)
                                             .Include(x => x.ShippingAddress)
                                             .Include(x => x.IdentityData)
                                             .Include(x => x.ShoppingCart)
                                                 .ThenInclude(x => x.LineItems)
                                                     .ThenInclude(x => x.Book)
                                                         .ThenInclude(x => x.Author)
                                             .Include(x => x.ShoppingCart)
                                                 .ThenInclude(x => x.LineItems)
                                                     .ThenInclude(x => x.Book)
                                                         .ThenInclude(x => x.Category)
                                             .FirstOrDefaultAsync(x => x.IdentityUserDataId == customerDto.UserId);
            var locationPricing = await _dbContext.FindLocationPricingByCountry(customerDto?.ShippingAddress.Country);
            customerExisting.ShippingAddress.EditAdress(_mapper.Map<Address>(customerDto.ShippingAddress), locationPricing);
            customerExisting.BillingAddress.EditAdress(_mapper.Map<Address>(customerDto.BillingAddress), null);
            customerExisting.FirstName = customerDto.FirstName;
            customerExisting.LastName = customerDto.LastName;
            return customerExisting;
        }
        private void EditCustomerInDatabase(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public Task ValidateRequest(EditProfile request)
        {
            throw new NotImplementedException();
        }
    }
}
