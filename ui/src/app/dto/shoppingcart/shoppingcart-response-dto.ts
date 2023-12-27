import { ShopItemResponseDto } from "./shopitem-response-dto";

export class ShoppingCartResponseDto {

    total: number;
    items: ShopItemResponseDto[]
    constructor(total?: number, items?: ShopItemResponseDto[]) {
        this.total = total ?? 0;
        this.items = items ?? [];
    }

}