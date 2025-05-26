import { Component } from '@angular/core';
import {BackButtonComponent} from '../../components/back-button/back-button.component';
import {Router} from '@angular/router';

@Component({
  selector: 'app-info',
  imports: [
    BackButtonComponent
  ],
  templateUrl: './info.component.html',
  styleUrl: './info.component.scss'
})
export class InfoComponent {
  constructor(private router: Router) {}
  onBackClicked() {
    this.router.navigate(['']);
  }
}
