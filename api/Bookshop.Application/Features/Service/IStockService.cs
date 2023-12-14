using Bookshop.Domain.Entities;

namespace Bookshop.Application.Features.Service
{
    public interface IStockService
    {
        bool UpdateSucceded {get; set;}
        bool CheckBookStockQuantities(ICollection<Book> books);
        void UpdateStockQuantities(ICollection<LineItem> lineItems);
        void RollbackStockQuantities(ICollection<LineItem> lineItems);
    }
}
