namespace FinancialBox.Shared.Contracts.Behaviors
{
    public interface IPipelineBehavior<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next);
    }
}
