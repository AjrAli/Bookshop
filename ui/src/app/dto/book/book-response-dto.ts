import { CommentResponseDto } from "./comment/comment-response-dto";

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
    authorAbout: string = '';
    categoryTitle: string = '';
    comments: CommentResponseDto[];

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
        this.authorAbout = data?.authorAbout ?? this.authorAbout;
        this.categoryTitle = data?.categoryTitle ?? this.categoryTitle;
        this.comments = data?.comments?.map(comment => new CommentResponseDto(comment)) ?? [];
    }
    get status(): string {
        if (this.quantity > 50)
            return 'INSTOCK';
        else if (this.quantity > 0 && this.quantity < 50)
            return 'LOWSTOCK';
        else if (this.quantity <= 0)
            return 'OUTOFSTOCK';
        return '';
    }
    get statusColor(): "success" | "secondary" | "info" | "warning" | "danger" | "contrast" | undefined {
        switch (this.status) {
            case 'INSTOCK':
                return 'success';
            case 'LOWSTOCK':
                return 'warning';
            case 'OUTOFSTOCK':
                return 'danger';
        }
        return undefined;
    }
}