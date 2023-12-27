export class BookResponseDto {
    title: string = '';
    description: string = '';
    publisher: string = '';
    isbn: string = '';
    price: number = 0;
    quantity: number = 0;
    pageCount: number = 0;
    dimensions: string = '';
    imageUrl: string = '';
    language: string = '';
    publishDate: string = '';
    authorName: string = '';
    categoryTitle: string = '';
  
    constructor(
      title?: string,
      description?: string,
      publisher?: string,
      isbn?: string,
      price?: number,
      quantity?: number,
      pageCount?: number,
      dimensions?: string,
      imageUrl?: string,
      language?: string,
      publishDate?: string,
      authorName?: string,
      categoryTitle?: string
    ) {
      // Initialize properties with default values if not provided
      this.title = title ?? this.title;
      this.description = description ?? this.description;
      this.publisher = publisher ?? this.publisher;
      this.isbn = isbn ?? this.isbn;
      this.price = price ?? this.price;
      this.quantity = quantity ?? this.quantity;
      this.pageCount = pageCount ?? this.pageCount;
      this.dimensions = dimensions ?? this.dimensions;
      this.imageUrl = imageUrl ?? this.imageUrl;
      this.language = language ?? this.language;
      this.publishDate = publishDate ?? this.publishDate;
      this.authorName = authorName ?? this.authorName;
      this.categoryTitle = categoryTitle ?? this.categoryTitle;
    }
    get status(): string {
        if (this.quantity > 50)
            return 'INSTOCK';
        else if (this.quantity < 50)
            return 'LOWSTOCK';
        else if (this.quantity === 0)
            return 'OUTOFSTOCK';
        return '';
    }
    get statusColor(): string {
        switch (this.status) {
            case 'INSTOCK':
                return 'success';
            case 'LOWSTOCK':
                return 'warning';
            case 'OUTOFSTOCK':
                return 'danger';
        }
        return '';
    }
}