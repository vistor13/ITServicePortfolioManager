import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { TaskFilteredRequest } from '../../interfaces/filtered-tasks-request';

@Component({
  selector: 'app-task-filter',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './task-filter.component.html',
  styleUrls: ['./task-filter.component.scss']
})
export class TaskFilterComponent {
  fromDate: string | null = null;
  toDate: string | null = null;
  sortDescending: boolean | null = null;
  algorithmName: string | null = null;

  isOpen = false;
  savedFilter: TaskFilteredRequest | null = null;

  @Output() filterChanged = new EventEmitter<TaskFilteredRequest | null>();

  toggleFilter() {
    this.isOpen = !this.isOpen;
  }

  applyFilter() {
    this.savedFilter = {
      fromDate: this.fromDate ? new Date(this.fromDate).toISOString() : null,
      toDate: this.toDate ? new Date(this.toDate).toISOString() : null,
      sortDescending: this.sortDescending,
      algorithmName: this.algorithmName
    };
    this.filterChanged.emit(this.savedFilter);
    this.isOpen = false;
  }
}
