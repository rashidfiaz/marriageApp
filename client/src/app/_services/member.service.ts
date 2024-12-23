import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from "../_models/Member";

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private http = inject(HttpClient);
  baseUrl = environment.ApiUrl;

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/'+ username);
  }

}

