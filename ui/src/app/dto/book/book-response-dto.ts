export class BookResponseDto {
    id: number = 0;
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

    constructor(data?: Partial<BookResponseDto>) {
        // Initialize properties with default values if not provided
        this.id = data?.id ?? this.id;
        this.title = data?.title ?? this.title;
        this.description = data?.description ?? this.description;
        this.publisher = data?.publisher ?? this.publisher;
        this.isbn = data?.isbn ?? this.isbn;
        this.price = data?.price ?? this.price;
        this.quantity = data?.quantity ?? this.quantity;
        this.pageCount = data?.pageCount ?? this.pageCount;
        this.dimensions = data?.dimensions ?? this.dimensions;
        this.imageUrl = data?.imageUrl ?? this.imageUrl;
        this.language = data?.language ?? this.language;
        this.publishDate = data?.publishDate ?? this.publishDate;
        this.authorName = data?.authorName ?? this.authorName;
        this.categoryTitle = data?.categoryTitle ?? this.categoryTitle;
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