import { ShopItemResponseDto } from "./shopitem-response-dto";

export class ShoppingCartResponseDto {

    total: number;
    items: ShopItemResponseDto[]

    constructor(data?: Partial<ShoppingCartResponseDto>) {
        this.total = data?.total ?? 0;
        this.items = data?.items?.map(item => new ShopItemResponseDto(item)) ?? [];
    }
    getTotalItems(): number {
        if (this.items) {
            const totalItems = this.items.reduce((sum, item) => sum + item.quantity, 0);
            return totalItems;
        }
        return 0;
    }
    updateTotal() {
        if (this.items && this.items.length > 0) {
            this.total = this.items.reduce((total, item) => total + item.price, 0);
        }else{
            this.total = 0 ;
        }
    }
    updateItems(newItems: ShopItemResponseDto[]): void {
        for (const newItem of newItems) {
            const existingItem = this.items.find(x => x.bookId === newItem.bookId);
            if (!existingItem) {
                this.items.push(newItem);
            }
        }
    }
    equals(other: ShoppingCartResponseDto | undefined): boolean {
        // Check if total and items are the same
        return (
          this.items.length === other?.items.length &&
          this.items.every((item, index) => item.equals(other?.items[index]))
        );
      }
}