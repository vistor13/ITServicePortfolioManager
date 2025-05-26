import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ResultResponse } from '../interfaces/result-response.model';
import {TaskRequest} from '../interfaces/task-request.model';

@Injectable({
  providedIn: 'root'
})
export class TaskStateService {
  private resultSubject = new BehaviorSubject<ResultResponse | null>(null);
  private taskSubject = new BehaviorSubject<TaskRequest | null>(null);
  result$ = this.resultSubject.asObservable();
  task$ = this.taskSubject.asObservable();
  private formingStartedSubject = new BehaviorSubject<boolean>(false);
  formingStarted$ = this.formingStartedSubject.asObservable();

  setFormingStarted(value: boolean): void {
    this.formingStartedSubject.next(value);
  }

  setResult(result: ResultResponse) {
    this.resultSubject.next(result);
  }

  setTask(task: TaskRequest) {
    this.taskSubject.next(task);
  }
  clearState() {
    this.resultSubject.next(null);
    this.taskSubject.next(null);
    this.formingStartedSubject.next(false);
  }
}
