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
    }
}