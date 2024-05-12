export class CommentDto {
    title: string = '';
    content: string = '';
    rating: number = 1;

    constructor(data?: Partial<CommentDto>) {
        this.title = data?.title ?? this.title;
        this.content = data?.content ?? this.content;
        this.rating = data?.rating ?? this.rating;
    }
}