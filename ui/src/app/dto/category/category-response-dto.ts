export class CategoryResponseDto {
    title: string;
    description: string;
    isVisible: boolean;

    constructor() {

        this.title = '';
        this.description = '';
        this.isVisible = true;
    }
}