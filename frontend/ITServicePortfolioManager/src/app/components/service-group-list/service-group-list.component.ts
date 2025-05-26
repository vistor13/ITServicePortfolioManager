import { Component, Input, Output, EventEmitter } from '@angular/core';

import { CommonModule } from '@angular/common';
import {ServiceGroupRequest} from '../../interfaces/task-request.model';

@Component({
  selector: 'app-service-group-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './service-group-list.component.html',
  styleUrl: './service-group-list.component.scss'
})
export class ServiceGroupListComponent {
  @Input() groups: ServiceGroupRequest[] = [];
  @Output() remove = new EventEmitter<number>();
}
