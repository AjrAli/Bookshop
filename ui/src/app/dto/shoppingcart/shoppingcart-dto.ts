import { ShopItemDto } from "./shopitem-dto";

export class ShoppingCartDto {

    items: ShopItemDto[]

    constructor() {
        this.items = [];
    }
}