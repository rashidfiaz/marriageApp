import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';;
import { User } from '../_models/User';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private http = inject(HttpClient);
  private baseUrl = "https://localhost:5001/api/";
  currentUer = signal<User | null>(null);

  login(model: any) {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUer.set(user);
        }
      })
    )
  }

  registerUser(model: any) {
    return this.http.post<User>(this.baseUrl + "account/register", model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUer.set(user);
        }
        return user;
      })
    )
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUer.set(null);
  }

}