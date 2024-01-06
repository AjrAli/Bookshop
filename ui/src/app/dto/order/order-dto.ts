export class OrderDto {

    methodOfPayment: string = '';

    constructor(data?: Partial<OrderDto>) {
        // Initialize properties with default values if not provided
        this.methodOfPayment = data?.methodOfPayment ?? this.methodOfPayment;
    }

}