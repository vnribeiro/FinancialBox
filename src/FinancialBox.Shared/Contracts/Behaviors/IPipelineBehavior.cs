namespace FinancialBox.Shared.Contracts.Behaviors
{
    public interface IPipelineBehavior<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, 
            Func<CancellationToken, Task<TResponse>> next, 
            CancellationToken cancellationToken);
    }
}
