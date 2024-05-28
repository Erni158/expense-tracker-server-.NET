using WebApplication2.Models;

namespace WebApplication2.DTOs
{
    public class ReceiptResponse
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Amount { get; set; }
        public List<ProductList> List { get; set; }
    }
}
