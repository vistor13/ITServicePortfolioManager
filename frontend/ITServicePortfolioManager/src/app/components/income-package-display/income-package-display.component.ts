import { Component, Input } from '@angular/core';
import {DecimalPipe, NgClass, NgForOf, NgIf} from '@angular/common';

@Component({
  selector: 'app-income-package-display',
  templateUrl: './income-package-display.component.html',
  imports: [
    DecimalPipe,
    NgIf,
    NgForOf,
    NgClass
  ],
  styleUrls: ['./income-package-display.component.scss']
})
export class IncomePackageDisplayComponent {
  @Input() data: any;
  @Input() customClass: string = '';
  dropdownOpen = false;

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  getTotalProvidersIncome(providersIncome: number[]): number {
    return providersIncome.reduce((sum, val) => sum + val, 0);
  }

  getProviderCount(portfolio: number[][]): number[] {
    return portfolio && portfolio.length > 0 ? portfolio[0].map((_, i) => i) : [];
  }

  getGroupCount(portfolio: number[][]): number[] {
    return portfolio ? portfolio.map((_, i) => i) : [];
  }
}
