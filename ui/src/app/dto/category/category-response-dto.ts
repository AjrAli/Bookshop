export class CategoryResponseDto {
    title: string = '';
    description: string = '';
    isVisible: boolean = true;
  
    constructor(title?: string, description?: string, isVisible?: boolean) {
      // Initialize properties with default values if not provided
      this.title = title ?? this.title;
      this.description = description ?? this.description;
      this.isVisible = isVisible ?? this.isVisible;
    }
}