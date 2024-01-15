export class CommentResponseDto {
    id: number = 0;
    title: string = '';
    content: string = '';
    rating: number = 1;
    dateComment: string = '';
    customerName: string = '';
    userName: string = '';


    constructor(data?: Partial<CommentResponseDto>) {
        // Initialize properties with default values if not provided
        this.id = data?.id ?? this.id;
        this.title = data?.title ?? this.title;
        this.content = data?.content ?? this.content;
        this.rating = data?.rating ?? this.rating;
        this.dateComment = data?.dateComment ?? this.dateComment;
        this.customerName = data?.customerName ?? this.customerName;
        this.userName = data?.userName ?? this.userName;
    }
}