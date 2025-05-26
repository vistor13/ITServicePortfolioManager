import {inject, Injectable} from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly themeKey = 'theme';
  cookieService : CookieService= inject(CookieService);
  constructor() {
    this.loadTheme();
  }

  setTheme(theme: string) {
    document.documentElement.setAttribute('data-theme', theme);
    this.cookieService.set(this.themeKey, theme, { expires: 365, path: '/' });
  }

  getTheme(): string {
    return this.cookieService.get(this.themeKey) || 'dark';
  }

  private loadTheme() {
    const savedTheme = this.getTheme();
    this.setTheme(savedTheme);
  }
}

