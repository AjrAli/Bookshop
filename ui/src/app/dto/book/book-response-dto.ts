export class BookResponseDto {
    title: string;
    description: string;
    publisher: string;
    isbn: string;
    price: number;
    quantity: number;
    pageCount: number;
    dimensions: string;
    imageUrl: string;
    language: string;
    publishDate: string;
    authorName: string;
    categoryTitle: string;
    constructor() {
        this.title = '';
        this.description = '';
        this.publisher = '';
        this.isbn = '';
        this.price = 0;
        this.quantity = 0;
        this.pageCount = 0;
        this.dimensions = '';
        this.imageUrl = '';
        this.language = '';
        this.publishDate = '';
        this.authorName = '';
        this.categoryTitle = '';
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