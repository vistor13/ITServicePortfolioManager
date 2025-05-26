import {Component, EventEmitter, Input, OnChanges, Output, SimpleChanges} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormArray,
  AbstractControl,
  ValidationErrors, FormControl
} from '@angular/forms';

@Component({
  selector: 'app-service-group-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './service-group-form.component.html',
  styleUrls: ['./service-group-form.component.scss']
})
export class ServiceGroupFormComponent implements OnChanges {
  @Input() providerCount: number = 1;
  @Output() addService = new EventEmitter<any>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      price: [null, [Validators.required, Validators.min(1)]],
      labourIntensity: [null, [Validators.required, Validators.min(0),this.integerValidator]],
      incomeForProviders: this.fb.array([])
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['providerCount']) {
      this.setIncomeInputs(this.providerCount);
    }
  }

  setIncomeInputs(count: number) {
    const incomes = this.form.get('incomeForProviders') as FormArray;
    incomes.clear();
    for (let i = 0; i < count; i++) {
      incomes.push(this.fb.control(null, [Validators.required, Validators.min(1)]));
    }
  }

  get priceControl() {
    return this.form.get('price') as FormControl ;
  }

  get labourIntensityControl() {
    return this.form.get('labourIntensity') as FormControl ;
  }

  get incomeControls() {
    return (this.form.get('incomeForProviders') as FormArray).controls;
  }

  submit() {
    if (this.form.valid) {
      this.addService.emit(this.form.value);
      this.form.reset();
      this.setIncomeInputs(this.providerCount);
    }
  }

  integerValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value == null || value === '') return null;
    return Number.isInteger(value) ? null : { notInteger: true };
  }
}
