import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { TaskRequest } from '../interfaces/task-request.model';
import { ResultResponse } from '../interfaces/result-response.model';
import { Observable, tap } from 'rxjs';
import {DiscountDeltaPopularServicesResponse, DiscountDeltaLowIncomeResponse} from '../interfaces/discount-delta-response';


@Injectable({
  providedIn: 'root'
})
export class SolverService {

  private http = inject(HttpClient);
  private cookieService = inject(CookieService);
  private apiUrl = 'http://localhost:5037/api/service-packages';

  createServicePortfolio(request: TaskRequest, typeAlgorithm: string): Observable<ResultResponse> {
    return this.http.post<ResultResponse>(`${this.apiUrl}/create`, request, {
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
  }

  getSolveId(typeAlgorithm: string): number | 0 {
    const idStr = typeAlgorithm === 'greedy'
      ? this.cookieService.get("resultGreedyId")
      : this.cookieService.get("resultGeneticId");

    const id = Number(idStr);
    return isNaN(id) ? 0 : id;
  }

  getSolutionById(idSolve: number): Observable<ResultResponse> {
    return this.http.get<ResultResponse>(`${this.apiUrl}`, {
      params: { idSolve: idSolve.toString() }
    });
  }

  getSolutionByTaskId(taskId: number): Observable<ResultResponse> {
    return this.http.get<ResultResponse>(`${this.apiUrl}/by-task`, {
      params: { taskId: taskId.toString() }
    });
  }
  applyDiscountsToPopularServices(
    request: TaskRequest,
    typeAlgorithm: string,
    id: number
  ): Observable<DiscountDeltaPopularServicesResponse> {
    return this.http.post<DiscountDeltaPopularServicesResponse>(
      `${this.apiUrl}/discounts/apply/popular`,
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
      `${this.apiUrl}/discounts/apply/low-income`,
      request,
      {
        params: {
          typeAlgorithm: typeAlgorithm,
          id: id.toString()
        }
      }
    );
  }
}
