import {Component, EventEmitter, inject, Output, Input, OnChanges, SimpleChanges }from '@angular/core';
import { SolverService } from '../../services/solver.service';
import { TaskResponse } from '../../interfaces/task-response.model';
import { NgForOf, NgIf } from '@angular/common';
import { TaskStateService } from '../../services/task-state.service';
import { TaskRequest } from '../../interfaces/task-request.model';
import { CookieService } from 'ngx-cookie-service';
import { TaskService } from '../../services/task.service';
import { TaskFilteredRequest } from '../../interfaces/filtered-tasks-request';

@Component({
  selector: 'app-list-tasks',
  standalone: true,
  imports: [NgForOf, NgIf],
  templateUrl: './list-tasks.component.html',
  styleUrl: './list-tasks.component.scss'
})
export class ListTasksComponent implements OnChanges {
  tasks: TaskResponse[] = [];
  selectedTask: TaskResponse | null = null;

  private cookieService = inject(CookieService);
  solveService = inject(SolverService);
  taskService = inject(TaskService);

  @Output() errorOccurred = new EventEmitter<string | null>();
  @Output() taskSelected = new EventEmitter<number>();
  @Input() filtered: TaskFilteredRequest | null = null;

  constructor(private taskStateService: TaskStateService) {}

  ngOnInit(): void {
    this.getTasks();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['filtered']) {
      this.getTasks();
    }
  }

  getTasks(): void {
    if (this.filtered) {
      this.taskService.getFilteredTasksByUserId(this.filtered).subscribe({
        next: (res) => this.tasks = res,
        error: (err) => this.errorOccurred.emit(err.error?.title)
      });
    } else {
      this.taskService.getTasksByUserId().subscribe({
        next: (res) => this.tasks = res,
        error: (err) => this.errorOccurred.emit(err.error?.title)
      });
    }
  }

  getById(taskId: number): void {
    const task = this.tasks.find(t => t.id === Number(taskId));
    if (!task) return;

    this.taskSelected.emit();
    this.solveService.getSolutionByTaskId(taskId).subscribe({
      next: (res) => {
        this.taskStateService.setResult(res);
        this.taskStateService.setSelectedTask(task);
        this.taskStateService.setTask(this.mapTaskResponseToRequest(task));
        this.taskStateService.setSelectedTypeAlgorithm(task.algorithm);

        if (task.id != null) {
          const cookieName = task.algorithm === 'greedy'
            ? "resultGreedyId"
            : task.algorithm === 'genetic'
              ? "resultGeneticId"
              : null;

          if (cookieName) {
            this.cookieService.set(cookieName, res.id.toString());
          }
        }
      }
    });
  }

  openModal(task: TaskResponse): void {
    this.selectedTask = task;
  }

  closeModal(): void {
    this.selectedTask = null;
  }

  mapTaskResponseToRequest(response: TaskResponse): TaskRequest {
    return {
      countProvider: response.countProvider,
      totalHumanResource: response.totalHumanResource,
      services: response.services.map((group, groupIndex) => ({
        services: group.serviceResponses.map(service => ({
          price: service.price,
          labourIntensity: service.labourIntensity,
          incomeForProviders: service.incomeForProviders,
          discount: service.discount,
          indexGroup: groupIndex
        }))
      }))
    };
  }
}
