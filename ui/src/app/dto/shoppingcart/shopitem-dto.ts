export class ShopItemDto {

    id: number = 0;
    quantity: number = 0;
    bookId: number = 0;
  
    constructor(id?: number, quantity?: number, bookId?: number) {
      // Initialize properties with default values if not provided
      this.id = id ?? this.id;
      this.quantity = quantity ?? this.quantity;
      this.bookId = bookId ?? this.bookId;
    }

}