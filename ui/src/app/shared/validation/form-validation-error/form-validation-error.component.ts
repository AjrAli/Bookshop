import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { AbstractControl, FormGroup, FormsModule, ValidationErrors, ValidatorFn } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-form-validation-error',
  imports: [FormsModule, CommonModule],
  templateUrl: './form-validation-error.component.html',
  styleUrls: ['./form-validation-error.component.css']
})
export class FormValidationErrorComponent {
  @Input() control!: AbstractControl | null;

}
export class PasswordMatchValidator {

  static match(controlName: string, matchingControlName: string): ValidatorFn {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const control = formGroup.get(controlName);
      const matchingControl = formGroup.get(matchingControlName);

      if (control?.value !== matchingControl?.value) {
        matchingControl?.setErrors({ mismatch: true });
        return { mismatch: true };
      } else {
        matchingControl?.setErrors(null);
        return null;
      }
    };
  }
}
