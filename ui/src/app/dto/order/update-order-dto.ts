
export class UpdateOrderDto {

    id: number;
    itemsId: number[]

    constructor(data?: Partial<UpdateOrderDto>) {
        this.id = data?.id ?? 0;
        this.itemsId = data?.itemsId ?? [];
    }

}