export class CategoryResponseDto {
  title: string = '';
  description: string = '';
  isVisible: boolean = true;

  constructor(data?: Partial<CategoryResponseDto>) {
    // Initialize properties with default values if not provided
    this.title = data?.title ?? this.title;
    this.description = data?.description ?? this.description;
    this.isVisible = data?.isVisible ?? this.isVisible;
  }
}