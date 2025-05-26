import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, tap, throwError} from 'rxjs';
import {CookieService} from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private cookieService = inject(CookieService);
  http = inject(HttpClient);

  token: string | null = null;
  private apiUrl = 'http://localhost:5037/api/auth';

  get isAuth(){
    if (this.token == null){
      this.token = this.cookieService.get('token');
    }
    return !!this.token;
  }

  logout() {
    this.cookieService.delete('token');
    this.token = null;
  }

  isLoggedIn(): boolean {
    return !!this.cookieService.get('token');
  }

  register(payload: { userName: string, password: string}) {
    return this.http.post(`${this.apiUrl}/register`, {
      userName: payload.userName,
      password: payload.password,
    }).pipe(
      catchError(error => {
        console.error('Registration error:', error);
        return throwError(error);
      })
    );
  }


  login(payload: { userName: string, password: string }) {
    return this.http.post<string>(`${this.apiUrl}/login`, {
      userName: payload.userName,
      password: payload.password
    }).pipe(
      tap(res => {
        this.token = res;
        this.cookieService.set('token', res);
      })
    );
  }

}
