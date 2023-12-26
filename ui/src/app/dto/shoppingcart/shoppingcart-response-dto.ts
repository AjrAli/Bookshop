import { ShopItemResponseDto } from "./shopitem-response-dto";

export class ShoppingCartResponseDto {

    total: number;
    items: ShopItemResponseDto[]
    constructor() {
        this.total = 0;
        this.items = [];
    }

}