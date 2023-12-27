import { ShopItemDto } from "./shopitem-dto";

export class ShoppingCartDto {

    items: ShopItemDto[]

    constructor(items?:ShopItemDto[]) {
        this.items = items ?? [];
    }
}