using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Application.Settings;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Application.Features.Customers.Queries.Authenticate
{
    public class AuthenticateHandler : IQueryHandler<Authenticate, AuthenticateResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();
        private readonly IMapper _mapper;

        public AuthenticateHandler(BookshopDbContext dbContext, UserManager<IdentityUserData> userManager,
            IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse> Handle(Authenticate request, CancellationToken cancellationToken)
        {
            if (request?.Username == null || request?.Password == null)
                throw new BadRequestException($"One of the credentials given is empty");
            var user = await _userManager.FindByNameAsync(request?.Username);

            if (user == null)
            {
                throw new NotFoundException($"Username : {request?.Username}");
            }

            var result = await _userManager.CheckPasswordAsync(user, request?.Password);
            if (!result)
            {
                throw new ValidationException($"Credentials for '{request?.Username} aren't valid'.");
            }
            var userRoles = await _userManager.GetRolesAsync(user); // Get the roles of the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            var jwtSecurityToken = JwtHelper.GenerateToken(user, userClaims, userRoles, _jwtSettings);
            var customer = _mapper.Map<CustomerDto>(_dbContext.Customers.Include(x => x.IdentityData).FirstOrDefault(x => x.IdentityUserDataId == user.Id));
            return new AuthenticateResponse
            {
                Message = $"User {request?.Username} successfully connected",
                Customer = customer,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken)
            };
        }
    }
}
