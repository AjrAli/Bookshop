import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { AbstractControl, FormsModule } from '@angular/forms';

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
