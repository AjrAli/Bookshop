﻿namespace Bookshop.Persistence.Contracts
{
    public interface ILoggedInUserService
    {
        public string? UserId { get; set; }
    }   
}