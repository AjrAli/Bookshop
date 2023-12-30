export class AuthorResponseDto {
  name: string = '';
  about: string = '';

  constructor(data?: Partial<AuthorResponseDto>) {
    // Initialize properties with default values if not provided
    this.name = data?.name ?? this.name;
    this.about = data?.about ?? this.about;
  }
}