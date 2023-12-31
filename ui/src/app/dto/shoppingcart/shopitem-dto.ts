export class ShopItemDto {

    id: number = 0;
    quantity: number = 0;
    bookId: number = 0;
  
    constructor(data?: Partial<ShopItemDto>) {
      // Initialize properties with default values if not provided
      this.id = data?.id ?? this.id;
      this.quantity = data?.quantity ?? this.quantity;
      this.bookId = data?.bookId ?? this.bookId;
    }

}