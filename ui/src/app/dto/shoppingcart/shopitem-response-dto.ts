export class ShopItemResponseDto {
  private readonly MAX_QUANTITY: number = 100;
  id: number = 0;
  quantity: number = 0;
  bookId: number = 0;
  price: number = 0;
  title: string = '';
  imageUrl: string = '';
  authorName: string = '';
  categoryTitle: string = '';

  constructor(data?: Partial<ShopItemResponseDto>) {
    this.id = data?.id ?? this.id;
    this.bookId = data?.bookId ?? this.bookId;
    this.price = data?.price ?? this.price;
    this.title = data?.title ?? this.title;
    this.imageUrl = data?.imageUrl ?? this.imageUrl;
    this.authorName = data?.authorName ?? this.authorName;
    this.categoryTitle = data?.categoryTitle ?? this.categoryTitle;
    this.setValidQuantity(data?.quantity);
  }

  setValidQuantity(quantity: number | undefined) {
    if (!quantity || quantity < 0) {
      this.quantity = 0;
      return;
    }
    if (quantity > this.MAX_QUANTITY)
      this.quantity = this.MAX_QUANTITY;
    this.quantity = quantity;
  }
  addQuantityWithLimit(quantity: number): boolean {
    if (this.quantity + quantity > this.MAX_QUANTITY) {
      this.quantity = this.MAX_QUANTITY;
      return false;
    }
    this.quantity += quantity;
    return true;
  }

}