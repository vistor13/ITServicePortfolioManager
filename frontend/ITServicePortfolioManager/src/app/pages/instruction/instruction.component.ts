import { Component } from '@angular/core';
import {BackButtonComponent} from '../../components/back-button/back-button.component';
import {Router} from '@angular/router';

@Component({
  selector: 'app-instruction',
  imports: [
    BackButtonComponent
  ],
  templateUrl: './instruction.component.html',
  styleUrl: './instruction.component.scss'
})
export class InstructionComponent {
  constructor(private router: Router) {}
  onBackClicked() {
    this.router.navigate(['']);
  }
}
