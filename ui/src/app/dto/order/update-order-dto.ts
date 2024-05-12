
export class UpdateOrderDto {

    itemsId: number[]

    constructor(data?: Partial<UpdateOrderDto>) {
        this.itemsId = data?.itemsId ?? [];
    }

}