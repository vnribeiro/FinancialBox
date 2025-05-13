namespace FinancialBox.Shared.ResultObjects
{
    public class Error
    {
        public IReadOnlyList<string> Messages { get; }

        public Error(params string[] messages)
        {
            Messages = messages?.ToList() ?? [];
        }

        public override string ToString()
        {
            return string.Join(" | ", Messages);
        }
    }
}
