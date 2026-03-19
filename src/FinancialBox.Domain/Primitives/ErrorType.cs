namespace FinancialBox.Domain.Primitives;

public enum ErrorType
{
    None,
    Validation,
    BusinessRule,
    Unauthenticated,
    Forbidden,
    NotFound,
    Conflict,
    TooManyRequests,
    InternalError,
    NotImplemented,
    ServiceUnavailable
}
