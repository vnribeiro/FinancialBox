using FinancialBox.Application.Common;

namespace FinancialBox.Application.Abstractions.Pipeline;

public interface IRequest<TResponse> where TResponse : IResult<TResponse>;
