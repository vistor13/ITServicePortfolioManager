import {Component, ViewChild} from '@angular/core';
import {ListTasksComponent} from '../../components/list-tasks/list-tasks.component';
import {BackButtonComponent} from '../../components/back-button/back-button.component';
import {ResultComponent} from '../../components/result/result.component';
import {Router} from '@angular/router';
import {TaskStateService} from '../../services/task-state.service';
import {SolverService} from '../../services/solver.service';

@Component({
  selector: 'app-user-tasks',
  imports: [
    ListTasksComponent,
    BackButtonComponent,
    ResultComponent
  ],
  templateUrl: './user-tasks.component.html',
  styleUrl: './user-tasks.component.scss'
})

export class UserTasksComponent {
  @ViewChild('resultComponent') resultComponent?: ResultComponent;
  constructor(private router: Router,private taskStateService: TaskStateService,private  solverService: SolverService) {}
  onBackClicked() {
    if (this.resultComponent) {
      this.resultComponent.reset();
      this.taskStateService.clearState();
      this.solverService.logout();
    }
    this.router.navigate(['']);
  }
}
