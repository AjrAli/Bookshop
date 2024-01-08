
export class EditPasswordDto {
    password: string = '';
    confirmPassword: string = '';
    newPassword: string = '';
    confirmNewPassword: string = '';

    constructor(data?: Partial<EditPasswordDto>) {
        // Initialize properties with default values if not provided
        this.password = data?.password ?? this.password;
        this.confirmPassword = data?.confirmPassword ?? this.confirmPassword;
        this.newPassword = data?.newPassword ?? this.newPassword;
        this.confirmNewPassword = data?.confirmNewPassword ?? this.confirmNewPassword;
    }
}