using FinancialBox.Application.Common;

namespace FinancialBox.Application.Contracts.Messaging;

public interface IRequest<TResponse> where TResponse : IResult<TResponse>;
