import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { TaskRequest } from '../interfaces/task-request.model';
import { ResultResponse } from '../interfaces/result-response.model';
import { Observable, tap } from 'rxjs';
import {DiscountDeltaPopularServicesResponse, DiscountDeltaLowIncomeResponse} from '../interfaces/discount-delta-response';
import {TaskResponse} from '../interfaces/task-response.model';


@Injectable({
  providedIn: 'root'
})
export class SolverService {

  private http = inject(HttpClient);
  private cookieService = inject(CookieService);
  private apiUrl = 'http://localhost:5037/api/portfolio';

  createServicePortfolio(request: TaskRequest, typeAlgorithm: string): Observable<ResultResponse> {
    return this.http.post<ResultResponse>(`${this.apiUrl}/solve`, request, {
      params: { typeAlgorithm }
    }).pipe(
      tap(res => {
        if (res && res.id != null) {
          if (typeAlgorithm === 'greedy') {
            this.cookieService.set("resultGreedyId", res.id.toString());
          } else if (typeAlgorithm === 'genetic') {
            this.cookieService.set("resultGeneticId", res.id.toString());
          }
        }
      })
    );
  }

  logout() {
    this.cookieService.delete('resultGreedyId');
    this.cookieService.delete('resultGeneticId');
    // опціонально: перевірка
    console.log('Cookies deleted:',
      this.cookieService.check('resultGreedyId'),
      this.cookieService.check('resultGeneticId')
    );
  }

  getSolveId(typeAlgorithm: string): number | 0 {
    const idStr = typeAlgorithm === 'greedy'
      ? this.cookieService.get("resultGreedyId")
      : this.cookieService.get("resultGeneticId");

    const id = Number(idStr);
    return isNaN(id) ? 0 : id;
  }

  getSolutionById(idSolve: number): Observable<ResultResponse> {
    return this.http.get<ResultResponse>(`${this.apiUrl}/solutions`, {
      params: { idSolve: idSolve.toString() }
    });
  }

  getSolutionByTaskId(taskId: number): Observable<ResultResponse> {
    return this.http.get<ResultResponse>(`${this.apiUrl}/solutions/by-task`, {
      params: { taskId: taskId.toString() }
    });
  }
  applyDiscountsToPopularServices(
    request: TaskRequest,
    typeAlgorithm: string,
    id: number
  ): Observable<DiscountDeltaPopularServicesResponse> {
    return this.http.post<DiscountDeltaPopularServicesResponse>(
      `${this.apiUrl}/apply-discounts/popular`,
      request,
      {
        params: {
          typeAlgorithm: typeAlgorithm,
          id: id.toString()
        }
      }
    );
  }

  applyDiscountsToLowIncomeProvider(
    request: TaskRequest,
    typeAlgorithm: string,
    id: number
  ): Observable<DiscountDeltaLowIncomeResponse> {
    return this.http.post<DiscountDeltaLowIncomeResponse>(
      `${this.apiUrl}/apply-discounts/low-income`,
      request,
      {
        params: {
          typeAlgorithm: typeAlgorithm,
          id: id.toString()
        }
      }
    );
  }

  getSolvesByUserId(): Observable<TaskResponse[]> {
    return this.http.get<TaskResponse[]>(`${this.apiUrl}/tasks`);
  }
}
