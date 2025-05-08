namespace FinancialBox.Shared.ResultObjects
{
    public class Error(string message)
    {
        public string Message { get; } = message;

        public override string ToString() => Message;
    }
}
