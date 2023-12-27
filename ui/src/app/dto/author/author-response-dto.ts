export class AuthorResponseDto {
    name: string = '';
    about: string = '';
  
    constructor(name?: string, about?: string) {
      // Initialize properties with default values if not provided
      this.name = name ?? this.name;
      this.about = about ?? this.about;
    }
}