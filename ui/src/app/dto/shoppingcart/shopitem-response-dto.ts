export class ShopItemResponseDto {

  id: number = 0;
  quantity: number = 0;
  bookId: number = 0;
  price: number = 0;
  title: string = '';
  imageUrl: string = '';
  authorName: string = '';
  categoryTitle: string = '';

  constructor(
    id?: number,
    quantity?: number,
    bookId?: number,
    price?: number,
    title?: string,
    imageUrl?: string,
    authorName?: string,
    categoryTitle?: string
  ) {
    // Initialize properties with default values if not provided
    this.id = id ?? this.id;
    this.quantity = quantity ?? this.quantity;
    this.bookId = bookId ?? this.bookId;
    this.price = price ?? this.price;
    this.title = title ?? this.title;
    this.imageUrl = imageUrl ?? this.imageUrl;
    this.authorName = authorName ?? this.authorName;
    this.categoryTitle = categoryTitle ?? this.categoryTitle;
  }

}