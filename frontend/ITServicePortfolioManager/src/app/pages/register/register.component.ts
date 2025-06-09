import {Component, inject} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {NgIf} from '@angular/common';


@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NgIf
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  router = inject(Router);
  authService = inject(AuthService);
  errorMessage: string | null = null;

  submitted = false;

  form: FormGroup = new FormGroup(
  {
    userName: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  },{ validators: this.passwordMatchValidator }
  );

  passwordMatchValidator(control: AbstractControl) {
  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  return password && confirmPassword && password !== confirmPassword
    ? { passwordMismatch: true }
    : null;
  }



  onSubmit() {
  this.submitted = true;
  this.form.markAllAsTouched();
  if (this.form.invalid) {
    return;
  }
  this.authService.register(this.form.value).subscribe({
    next: () => this.router.navigate(['/login']),
    error: (err) => {
      this.errorMessage = err.error?.title;
    }
  });
}
}
