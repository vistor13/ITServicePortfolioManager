import { Component, EventEmitter, Output, Input } from '@angular/core';
import { NgIf } from '@angular/common';
import { TaskFilteredRequest } from '../../interfaces/filtered-tasks-request';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [NgIf],
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent {
  @Input() savedFilter: TaskFilteredRequest | null = null;

  fromDate: string | null = null;
  toDate: string | null = null;
  sortDescending: boolean | null = null;
  algorithmName: string | null = null;

  @Output() filterChanged = new EventEmitter<TaskFilteredRequest | null>();

  clearFilter() {
    this.fromDate = null;
    this.toDate = null;
    this.sortDescending = null;
    this.algorithmName = null;
    this.savedFilter = null;
    this.filterChanged.emit(null);
  }

  hasActiveFilter(): boolean {
    const f = this.savedFilter;
    return !!(f?.fromDate || f?.toDate || f?.algorithmName || f?.sortDescending);
  }
  isFilterInfoOpen = false;

  openFilterInfo() {
    this.isFilterInfoOpen = true;
  }

  closeFilterInfo() {
    this.isFilterInfoOpen = false;
  }

  getFilterSummaryShort(): string {
    const full = this.getFilterSummary();
    if (full.length > 50) {
      return full.slice(0, 47) + '...';
    }
    return full;
  }

  private formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // місяці з 0
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }

  getFilterSummary(): string {
    const f = this.savedFilter;
    if (!f) return '';
    const parts = [];
    if (f.fromDate) parts.push(`від ${this.formatDate(f.fromDate)}`);
    if (f.toDate) parts.push(`до ${this.formatDate(f.toDate)}`);
    if (f.algorithmName) parts.push(`алгоритм: ${f.algorithmName}`);
    if (f.sortDescending !== null && f.sortDescending !== undefined) {
      parts.push(`за датою створення: ${f.sortDescending ? 'новіші' : 'старіші'}`);
    }
    return parts.join(', ');
  }


}
