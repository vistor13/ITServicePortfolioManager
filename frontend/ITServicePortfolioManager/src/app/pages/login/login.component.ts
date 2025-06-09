import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NgIf
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})

export class LoginComponent {
  router = inject(Router);
  authService= inject(AuthService);
  errorMessage: string | null = null;
  submitted = false;
  form: FormGroup = new FormGroup({
    userName: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  onSubmit() {
    this.form.markAllAsTouched();
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    this.authService.login(this.form.value).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: (err) => {
        this.errorMessage = err.error?.title;
      }
    });
  }
}
