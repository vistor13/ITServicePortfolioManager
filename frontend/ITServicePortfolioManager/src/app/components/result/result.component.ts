import {Component, inject, OnInit} from '@angular/core';
import {ResultResponse} from '../../interfaces/result-response.model';
import {TaskStateService} from '../../services/task-state.service';
import {DecimalPipe, NgClass, NgForOf, NgIf} from '@angular/common';
import {TaskRequest} from '../../interfaces/task-request.model';
import {SolverService} from '../../services/solver.service';
import {
  DiscountDeltaLowIncomeResponse,
  DiscountDeltaPopularServicesResponse,
  ResultWithDiscountResponse
} from '../../interfaces/discount-delta-response';
import {DiscountChartComponent} from '../discount-chart/discount-chart.component';
import {IncomePackageDisplayComponent} from '../income-package-display/income-package-display.component';
import {firstValueFrom} from 'rxjs';

@Component({
  selector: 'app-result',
  standalone: true,
  imports: [
    NgIf,
    NgClass,
    DiscountChartComponent,
    IncomePackageDisplayComponent,
    DecimalPipe,
    NgForOf
  ],
  templateUrl: './result.component.html',
  styleUrl: './result.component.scss'
})
export class ResultComponent implements OnInit {
  result: ResultResponse | null = null;
  task: TaskRequest | null = null;
  solveService: SolverService= inject(SolverService);
  selectedAlgorithm= 'genetic';
  discountDeltasMostPopularServices: DiscountDeltaPopularServicesResponse| null = null;
  discountDeltasLowIncomeProvider: DiscountDeltaLowIncomeResponse| null = null;
  showChart = false;
  resultWithDiscountMorePopularDeltaResponse: ResultWithDiscountResponse | null | undefined = null;
  resultWithDiscountLowIncomeDeltaResponse: ResultWithDiscountResponse | null | undefined = null;
  loading = true;

  isFormingStarted = false;

  constructor(private taskStateService: TaskStateService) {}

  ngOnInit(): void {

    this.taskStateService.formingStarted$.subscribe(started => {
      this.isFormingStarted = started;
    });

    this.taskStateService.result$.subscribe(res => {
      this.result = res;
    });

    this.taskStateService.task$.subscribe(task => {
      this.task = task;
    });
  }
  reset(): void {
    this.result = null;
    this.task = null;
    this.selectedAlgorithm='genetic';
    this.discountDeltasMostPopularServices=null;
    this.resultWithDiscountLowIncomeDeltaResponse = null;
    this.showChart = false;
    this.loading = true;
    this.isFormingStarted = false;
    this.resultWithDiscountLowIncomeDeltaResponse=null;
    this.resultWithDiscountMorePopularDeltaResponse = null;
  }


  getNameChart(chartType: 'mostPopularGeneral' | 'mostPopularDetailed' | 'lowIncomeGeneral' | 'lowIncomeDetailed'): string {
    switch (chartType) {
      case 'mostPopularGeneral':
        return 'Знижки на найвживаніші групи сервісів';
      case 'mostPopularDetailed':
        return 'Детальний огляд на знижки найвживаніших груп сервісів';
      case 'lowIncomeGeneral':
        return 'Знижки для провайдерів з найнижчим доходом (загальний огляд)';
      case 'lowIncomeDetailed':
        return 'Детальний огляд знижок для провайдерів з найнижчим доходом';
      default:
        return 'Графік знижок';
    }
  }


  async showDiscountChart() {
    this.showChart = true;
    this.loading = true;

    if (this.task) {
      try {
        await this.applyDiscountsToPopularServices(this.task, this.selectedAlgorithm);
        await this.applyDiscountsToLowIncomeProvider(this.task, this.selectedAlgorithm);
      } catch (error) {
        console.error('Помилка при застосуванні знижок:', error);
      } finally {
        this.loading = false;
      }
    }
  }


  async applyDiscountsToPopularServices(request: TaskRequest, typeAlgorithm: string) {
    const id = this.solveService.getSolveId(typeAlgorithm);
    const res = await firstValueFrom(
      this.solveService.applyDiscountsToPopularServices(request, typeAlgorithm, id)
    );
    this.discountDeltasMostPopularServices = res;
    this.resultWithDiscountMorePopularDeltaResponse = res.bestResult;
  }

  async applyDiscountsToLowIncomeProvider(request: TaskRequest, typeAlgorithm: string) {
    const id = this.solveService.getSolveId(typeAlgorithm);
    const res = await firstValueFrom(
      this.solveService.applyDiscountsToLowIncomeProvider(request, typeAlgorithm, id)
    );
    this.discountDeltasLowIncomeProvider = res;
    this.resultWithDiscountLowIncomeDeltaResponse = res.bestResult;
  }


    getPortfolio(request: TaskRequest, typeAlgorithm: string) {
    const id = this.solveService.getSolveId(typeAlgorithm);
    if (id === 0) {
      this.solveService.createServicePortfolio(request, typeAlgorithm).subscribe(res => {
        this.result = res;
        this.selectedAlgorithm = typeAlgorithm;
      });
    } else {
      this.solveService.getSolverById(id).subscribe({
        next: res => {
          this.result = res;
          this.selectedAlgorithm = typeAlgorithm;
        }
      });
    }
    this.discountDeltasMostPopularServices = null;
    this.showChart = false;
    this.discountDeltasLowIncomeProvider = null;
    this.resultWithDiscountLowIncomeDeltaResponse = null;
    this.resultWithDiscountMorePopularDeltaResponse = null;
    this.loading = false;
  }

}
