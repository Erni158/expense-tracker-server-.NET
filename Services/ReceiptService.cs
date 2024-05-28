using MongoDB.Driver;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class ReceiptService
    {
        private readonly IMongoCollection<Receipt> _receipt;

        public ReceiptService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("expense-tracker-db");
            _receipt = database.GetCollection<Receipt>("Receipts");
        }

        public async Task<List<Receipt>> GetReceiptsAsync(string userId, DateTime? start, DateTime? end)
        {
            var filter = Builders<Receipt>.Filter.Eq("user", userId);

            if (start.HasValue && end.HasValue)
            {
                var dateFilter = Builders<Receipt>.Filter.Gte("createdAt", start.Value) &
                                 Builders<Receipt>.Filter.Lt("createdAt", end.Value);
                filter = Builders<Receipt>.Filter.And(filter, dateFilter);
            }

            var receipts = await _receipt.Find(filter).ToListAsync();
            return receipts;
        }

        public async Task<Receipt> AddReceiptAsync(Receipt receipt)
        {
            await _receipt.InsertOneAsync(receipt);
            return receipt;
        }

        public async Task DeleteReceiptsAsync(IEnumerable<string> ids)
        {
            var filter = Builders<Receipt>.Filter.In(r => r.Id, ids);
            await _receipt.DeleteManyAsync(filter);
        }
    }
}
