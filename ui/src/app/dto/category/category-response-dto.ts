export class CategoryResponseDto {
  id: number = 0;
  title: string = '';
  description: string = '';
  isVisible: boolean = true;

  constructor(data?: Partial<CategoryResponseDto>) {
    this.id = data?.id ?? this.id;
    this.title = data?.title ?? this.title;
    this.description = data?.description ?? this.description;
    this.isVisible = data?.isVisible ?? this.isVisible;
  }
}