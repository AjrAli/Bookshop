import { ShopItemResponseDto } from "./shopitem-response-dto";

export class ShoppingCartResponseDto {

    total: number;
    items: ShopItemResponseDto[]

    constructor(total?: number, items?: ShopItemResponseDto[]) {
        this.total = total ?? 0;
        this.items = items ?? [];
    }
    getTotalItems(): number {
        if (this.items) {
            const totalItems = this.items.reduce((sum, item) => sum + item.quantity, 0);
            return totalItems;
        }
        return 0;
    }
}