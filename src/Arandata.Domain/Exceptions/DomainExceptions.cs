using System;

namespace Arandata.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string? message = null) : base(message) { }
    }

    public class NotFoundException : DomainException
    {
        public NotFoundException(string? message = null) : base(message) { }
    }

    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string? message = null) : base(message) { }
    }

    public class DuplicateEntityException : DomainException
    {
        public DuplicateEntityException(string? message = null) : base(message) { }
    }
}
