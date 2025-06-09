import {Component, ViewChild} from '@angular/core';
import {ResultComponent} from '../../components/result/result.component';
import {TaskFormComponent} from '../../components/task-form/task-form.component';
import {BackButtonComponent} from '../../components/back-button/back-button.component';
import {Router} from '@angular/router';
import {TaskStateService} from '../../services/task-state.service';
import {SolverService} from '../../services/solver.service';

@Component({
  selector: 'app-service-portfolio-builder',
  standalone: true,
  imports: [
    ResultComponent,
    TaskFormComponent,
    BackButtonComponent
  ],
  templateUrl: './service-portfolio-builder.component.html',
  styleUrl: './service-portfolio-builder.component.scss'
})
export class ServicePortfolioBuilderComponent {
  @ViewChild('resultComponent') resultComponent?: ResultComponent;

  constructor(private router: Router,private taskStateService: TaskStateService,private  solverService: SolverService) {}

  newFormating() {
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

}
