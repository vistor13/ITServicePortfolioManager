import {inject, Injectable} from '@angular/core';
import {catchError, Observable, throwError} from 'rxjs';
import {TaskResponse} from '../interfaces/task-response.model';
import {HttpClient} from '@angular/common/http';
import {TaskFilteredRequest} from '../interfaces/filtered-tasks-request';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5037/api/task';
  constructor() { }

  getTasksByUserId(): Observable<TaskResponse[]> {
    return this.http.get<TaskResponse[]>(`${this.apiUrl}/tasks`).pipe(
      catchError(error => {
        return throwError(() => error);
      })
    );
  }

  getFilteredTasksByUserId(filter: TaskFilteredRequest): Observable<TaskResponse[]> {
    return this.http.post<TaskResponse[]>(`${this.apiUrl}/filtered-tasks`, filter).pipe(
      catchError(error => throwError(() => error))
    );
  }
}
