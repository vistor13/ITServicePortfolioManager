import { inject } from '@angular/core';
import { Router } from '@angular/router';
import {AuthService} from '../services/auth.service';

export const NoAuthGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuth()) {
    return router.createUrlTree(['']);
  }

  return true;
};
