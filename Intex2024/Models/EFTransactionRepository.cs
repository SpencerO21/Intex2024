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

        public IQueryable<Transaction> Transactions => context.Transactions;
                            

        public IQueryable<Transaction> Transcations => throw new NotImplementedException();

        public void SaveOrder(Transaction transaction, List<LineItem> lineItems)
        {
            // for loop to Add line items to database

            
            if (transaction.TransactionId == 0)
            {
                context.Transactions.Add(transaction);
            }
            context.SaveChanges();
        }

        
    }
}