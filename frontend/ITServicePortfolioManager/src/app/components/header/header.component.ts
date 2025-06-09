import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {NgIf} from '@angular/common';
import {SolverService} from '../../services/solver.service';

@Component({
  selector: 'app-header',
  standalone:true,
  imports: [
    RouterLink,
    NgIf
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  authService=inject(AuthService);
  solverService=inject(SolverService);
  logout(){
    this.authService.logout();
    this.solverService.logout();
    window.location.reload();
  }
  isAuthenticated(): boolean {
    return this.authService.isAuth();
  }
}
