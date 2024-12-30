
import { Component, inject, OnInit, signal, Signal } from '@angular/core';
import { Member } from '../../_models/Member';
import { MemberService } from '../../_services/member.service';
import { MemberCardComponent } from "../member-card/member-card.component";

@Component({
  selector: 'app-members-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css'
})
export class MembersListComponent implements OnInit {
  
  public memberService = inject(MemberService);
  
  ngOnInit(): void {
    if (this.memberService.members().length == 0) {
        this.getMembers();
    }
  }

  getMembers() {
    this.memberService.getMembers();
  }
}
