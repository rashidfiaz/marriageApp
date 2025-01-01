import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { JsonPipe, NgIf } from '@angular/common';
import { validateHorizontalPosition } from '@angular/cdk/overlay';
import { InputTextComponent } from '../_forms/input-text/input-text.component';
import { DatePickerComponent } from '../_forms/date-picker/date-picker.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, InputTextComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  //getUsersFromtheHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();
  registerForm: FormGroup = new FormGroup({});
  maxDate = new Date();
  validationErrors = [];

  ngOnInit(): void {
      this.initializeForm();
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group ({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required,Validators.minLength(4), Validators.required,Validators.maxLength(8) ]],
      confirmPassword: ['', [Validators.required,Validators.minLength(4), Validators.required,Validators.maxLength(8), this.matchValues('password') ]]
    })
    this.registerForm.controls['password']?.valueChanges.subscribe({
      next: () => this.registerForm.controls["confirmPassword"].updateValueAndValidity()
    })
  }

  registerUser() {
    const dob = this.getDateOnly(this.registerForm.get("dateOfBirth")?.value)
    this.registerForm.patchValue({dateOfBirth: dob}); //Patch the updated value with form fields
    this.accountService.registerUser(this.registerForm.value).subscribe({
      next: _ => this.router.navigateByUrl('/members'),
      error: error => this.validationErrors = error
    })
     
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null: {isMatching: true}
    }
  }

  //This method convert the date from full date with timestamp to Date only

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    return new Date(dob).toISOString().slice(0, 10);
  }
}
