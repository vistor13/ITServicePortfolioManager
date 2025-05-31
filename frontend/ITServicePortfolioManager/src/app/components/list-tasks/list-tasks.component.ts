import { Component, inject } from '@angular/core';
import { SolverService } from '../../services/solver.service';
import { TaskResponse } from '../../interfaces/task-response.model';
import { NgForOf, NgIf } from '@angular/common';
import {TaskStateService} from '../../services/task-state.service';
import {TaskRequest} from '../../interfaces/task-request.model';
import {CookieService} from 'ngx-cookie-service';

@Component({
  selector: 'app-list-tasks',
  standalone: true,
  imports: [NgForOf, NgIf],
  templateUrl: './list-tasks.component.html',
  styleUrl: './list-tasks.component.scss'
})
export class ListTasksComponent {
  tasks: TaskResponse[] = [];
  selectedTask: TaskResponse | null = null;
  private cookieService = inject(CookieService);
  solveService = inject(SolverService);
  constructor(private taskStateService: TaskStateService) {}

  ngOnInit(): void {
    this.getTasksByUserId();
  }

  getById(taskId: number): void {
    const task = this.tasks.find(t => t.id === Number(taskId));
    if (!task) {
      console.error('Задача з таким ID не знайдена');
      return;
    }
    this.solveService.getSolutionByTaskId(taskId).subscribe({
      next: (res) => {
        this.taskStateService.setResult(res);
        this.taskStateService.setSelectedTask(task)
        this.taskStateService.setTask(this.mapTaskResponseToRequest(task))
        this.taskStateService.setSelectedTypeAlgorithm(task.algorithm);
        if (task && task.id != null) {
          if (task.algorithm === 'greedy') {
            this.cookieService.set("resultGreedyId", res.id.toString());
          } else if (task.algorithm === 'genetic') {
            this.cookieService.set("resultGeneticId", res.id.toString());
          }
        }
      }
    });

  }



  getTasksByUserId(): void {
    this.solveService.getSolvesByUserId().subscribe(res => {
      this.tasks = res;
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
