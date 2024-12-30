import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from "../_models/Member";
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private http = inject(HttpClient);
  members = signal<Member[]>([]);
  
  baseUrl = environment.ApiUrl;

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
      next: members => this.members.set(members)
    });
  }

  getMember(username: string) {
    const member = this.members().find(x=>x.username == username);
    if (member != undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/'+ username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {
        this.members.update(members => members.map(m => m.username == member.username ? member: m))
      })
    );
  }

}

