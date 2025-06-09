import {Component, EventEmitter, inject, OnInit, Output} from '@angular/core';
import {
  FormBuilder,
  Validators,
  FormGroup,
  ReactiveFormsModule,
  FormControl,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ServiceGroupListComponent } from '../service-group-list/service-group-list.component';
import { ServiceGroupFormComponent } from '../service-group-form/service-group-form.component';
import { ServiceRequest, TaskRequest, ServiceGroupRequest } from '../../interfaces/task-request.model';
import { SolverService } from '../../services/solver.service';
import { ResultResponse } from '../../interfaces/result-response.model';
import { TaskStateService } from '../../services/task-state.service';
import {CookieService} from 'ngx-cookie-service';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ServiceGroupListComponent,
    ServiceGroupFormComponent
  ],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent implements OnInit {
  form!: FormGroup;
  discountform!: FormGroup;

  serviceGroups: ServiceGroupRequest[] = [];
  currentServices: ServiceRequest[] = [];
  isGroupFormOpen = false;
  attemptedSubmit = false;
  submitted = false;

  result?: ResultResponse;

  solveService: SolverService = inject(SolverService);
  taskStateService: TaskStateService = inject(TaskStateService);
  cookieService = inject(CookieService);
  @Output() newFormating = new EventEmitter<number>();

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      countProvider: [null, [Validators.required, Validators.min(1),this.integerValidator]],
      totalHumanResource: [null, [Validators.required, Validators.min(1),this.integerValidator]],
      hasDiscount: [false]
    });

    this.discountform = this.fb.group({
      discount: [{ value: null, disabled: true }, [Validators.required, Validators.min(1), Validators.max(100)]]
    });

    this.form.get('hasDiscount')?.valueChanges.subscribe((hasDiscount) => {
      const discountControl = this.discountform.get('discount');
      if (hasDiscount) {
        discountControl?.enable();
      } else {
        discountControl?.disable();
      }
    });
  }


  get countProviderControl(): FormControl {
    return this.form.get('countProvider') as FormControl;
  }

  get totalHumanResourceControl(): FormControl {
    return this.form.get('totalHumanResource') as FormControl;
  }

  get discountControl(): FormControl {
    return this.discountform.get('discount') as FormControl;
  }



  openAddGroup() {
    this.submitted = true;
    if (this.countProviderControl.invalid || !this.countProviderControl.value) {
      return;
    }

    this.currentServices = [];
    this.isGroupFormOpen = true;
  }


  addServiceToGroup(service: ServiceRequest) {
    this.currentServices.push(service);
  }

  saveGroup() {
    const newGroupIndex = this.serviceGroups.length + 1;
    const hasDiscount = this.form.get('hasDiscount')?.value;
    const discount = hasDiscount ? (this.discountform.get('discount')?.value ?? 0) / 100 : 0;

    const servicesWithDetails = this.currentServices.map((s) => ({
      ...s,
      indexGroup: newGroupIndex,
      discount: discount
    }));

    this.serviceGroups.push({ services: servicesWithDetails });

    this.isGroupFormOpen = false;
    this.discountform.reset({ discount: 0 });
  }
  canCreatePortfolio(): boolean {
    return (
      this.form.valid &&
      this.form.value.countProvider > 0 &&
      this.form.value.totalHumanResource > 0 &&
      this.serviceGroups.length > 0
    );
  }

  closeAddGroup() {
    this.currentServices = [];
    this.isGroupFormOpen = false;
  }

  removeGroup(index: number) {
    this.serviceGroups.splice(index, 1);
  }

  getTaskRequest(): TaskRequest {
    return {
      countProvider: this.form.value.countProvider,
      totalHumanResource: this.form.value.totalHumanResource,
      services: this.serviceGroups
    };
  }

  createPortfolio(request: TaskRequest, typeAlgorithm: string) {
    this.attemptedSubmit = true;

    if (!this.canCreatePortfolio()) {
      return;
    }
    this.newFormating.emit();
    this.cookieService.delete("resultGreedyId");
    this.cookieService.delete("resultGeneticId");
    this.taskStateService.setFormingStarted(true);
    this.solveService.createServicePortfolio(request, typeAlgorithm).subscribe(res => {
      this.result = res;
      this.taskStateService.setResult(res);
      this.taskStateService.setTask(request);
      this.taskStateService.setSelectedTypeAlgorithm(typeAlgorithm);
    });
  }
   integerValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value == null || value === '') return null;
    return Number.isInteger(value) ? null : { notInteger: true };
  }

}

