import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-back-button',
  standalone:true,
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.scss'
})
export class BackButtonComponent {
  @Output() backClick = new EventEmitter<void>();

  onClick() {
    this.backClick.emit();

  }
}
