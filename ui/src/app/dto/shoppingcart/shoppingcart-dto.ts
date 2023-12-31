import { ShopItemDto } from "./shopitem-dto";

export class ShoppingCartDto {

    items: ShopItemDto[]

    constructor(data?: Partial<ShoppingCartDto>) {
        this.items = data?.items?.map(item => new ShopItemDto(item)) ?? [];
    }

}