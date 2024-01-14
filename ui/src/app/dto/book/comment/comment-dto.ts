export class CommentDto {

    id: number = 0;
    title: string = '';
    content: string = '';
    rating: number = 1;
    bookId: number = 0;

    constructor(data?: Partial<CommentDto>) {
        this.id = data?.id ?? this.id;
        this.title = data?.title ?? this.title;
        this.content = data?.content ?? this.content;
        this.rating = data?.rating ?? this.rating;
        this.bookId = data?.bookId ?? this.bookId;
    }
}