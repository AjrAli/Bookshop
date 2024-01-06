import { ShopItemResponseDto } from "../shoppingcart/shopitem-response-dto";


export class OrderResponseDto {

    id: number;
    total: number;
    shippingFee: number;
    vatRate: number;
    statusOrder: string;
    dateOrder: string;
    methodOfPayment: string;
    items: ShopItemResponseDto[]

    constructor(data?: Partial<OrderResponseDto>) {
        this.id = data?.id ?? 0;
        this.total = data?.total ?? 0;
        this.shippingFee = data?.shippingFee ?? 0;
        this.vatRate = data?.vatRate ?? 0;
        this.statusOrder = data?.statusOrder ?? '';
        this.dateOrder = data?.dateOrder ?? '';
        this.methodOfPayment = data?.methodOfPayment ?? '';
        this.items = data?.items?.map(item => new ShopItemResponseDto(item)) ?? [];
    }

}