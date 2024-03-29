﻿
namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShopItemResponseDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
        public decimal BookPrice { get; set; }
        public decimal Price { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryTitle { get; set; }
    }
}
