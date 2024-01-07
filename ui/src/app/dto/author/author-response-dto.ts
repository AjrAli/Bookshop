export class AuthorResponseDto {
  id: number = 0;
  name: string = '';
  about: string = '';

  constructor(data?: Partial<AuthorResponseDto>) {
    this.id = data?.id ?? this.id;
    this.name = data?.name ?? this.name;
    this.about = data?.about ?? this.about;
  }
}