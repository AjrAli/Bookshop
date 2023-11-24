﻿namespace Bookshop.Application.Exceptions
{

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }
    }
}
