﻿namespace Bookshop.Application.Features.Books
{
    public class BookResponseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Isbn { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PageCount { get; set; }
        public string Dimensions { get; set; }
        public string Language { get; set; }
        public string PublishDate { get; set; }
    }
}
