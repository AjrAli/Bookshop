export class ShopItemResponseDto {

    id: number;
    quantity: number;
    bookId: number;
    price: number;
    title: string;
    imageUrl: string;
    authorName: string;
    categoryTitle: string;
    constructor() {
        this.id = 0;
        this.quantity = 0;
        this.bookId = 0;
        this.price = 0;
        this.title = '';
        this.imageUrl = '';
        this.authorName = '';
        this.categoryTitle = '';
    }

}