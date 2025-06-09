import {Component, ViewChild} from '@angular/core';
import {ListTasksComponent} from '../../components/list-tasks/list-tasks.component';
import {BackButtonComponent} from '../../components/back-button/back-button.component';
import {ResultComponent} from '../../components/result/result.component';
import {Router} from '@angular/router';
import {TaskStateService} from '../../services/task-state.service';
import {SolverService} from '../../services/solver.service';
import {NgIf} from '@angular/common';
import {TaskFilteredRequest} from '../../interfaces/filtered-tasks-request';
import {TaskFilterComponent} from '../../components/task-filter/task-filter.component';
import {FilterComponent} from '../../components/filter/filter.component';

@Component({
  selector: 'app-user-tasks',
  imports: [
    ListTasksComponent,
    BackButtonComponent,
    ResultComponent,
    NgIf,
    TaskFilterComponent,
    FilterComponent
  ],
  templateUrl: './user-tasks.component.html',
  styleUrl: './user-tasks.component.scss'
})

export class UserTasksComponent {
  errorMessage: string | null = null;
  currentFilter: TaskFilteredRequest | null = null;

  @ViewChild('resultComponent') resultComponent?: ResultComponent;

  constructor(
    private router: Router,
    private taskStateService: TaskStateService,
    private solverService: SolverService
  ) {}

  onErrorOccurred(message: string | null) {
    this.errorMessage = message;
  }

  onTaskSelected() {
    if (this.resultComponent) {
      this.resultComponent.reset();
    }
  }

  onBackClicked() {
    if (this.resultComponent) {
      this.resultComponent.reset();
      this.taskStateService.clearState();
      this.solverService.logout();
    }
    this.router.navigate(['']);
  }

  onFilterChanged(filter: TaskFilteredRequest | null) {
    this.currentFilter = filter;
  }

}
