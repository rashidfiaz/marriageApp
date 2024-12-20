import { Component, inject, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  
  accountService = inject(AccountService);
  
  //getUsersFromtheHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();
  private toastr = inject(ToastrService);

  model: any = {};

  registerUser() {
    this.accountService.registerUser(this.model).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error: error => this.toastr.error(error.error)
    })
     
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
