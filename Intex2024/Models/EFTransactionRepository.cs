using Microsoft.EntityFrameworkCore;

namespace Intex2024.Models
{

    public class EFTransactionRepository : ITransactionRepository
    {
        private IntexStoreContext context;

        public EFTransactionRepository(IntexStoreContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Transaction> Transactions => context.Transactions
                            .Include(o => o.Lines)
                            .ThenInclude(l => l.Product);

        public IQueryable<Transaction> Transcations => throw new NotImplementedException();

        public void SaveOrder(Transaction transaction)
        {
            context.AttachRange(transaction.Lines.Select(l => l.Product));
            if (transaction.TransactionId == 0)
            {
                context.Transactions.Add(transaction);
            }
            context.SaveChanges();
        }

        
    }
}