namespace Intex2024.Models
{
    public interface ITransactionRepository
    {
        IQueryable<Transaction> Transcations { get; }
        void SaveOrder(Transaction transaction);
    }
}
