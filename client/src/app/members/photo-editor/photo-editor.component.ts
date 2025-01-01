import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/Member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { MemberService } from '../../_services/member.service';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, NgIf, NgStyle, FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  private memberService = inject(MemberService);
  private baseUrl = environment.ApiUrl;

  uploader?:FileUploader;
  member = input.required<Member>();
  memberChange = output<Member>();
  hasBaseDropZoneOver = false;
  

  ngOnInit(): void {
      this.initializeUploader();
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo).subscribe({
      next: _ => {
        this.updateMainPhoto(photo);
      }
    })
  }

  deletePhoto(photo: Photo){
    this.memberService.deletePhoto(photo).subscribe({
      next: _ => {
          const updateMember = {...this.member()};
          updateMember.photos = updateMember.photos.filter(x=>x.id !== photo.id);
          this.memberChange.emit(updateMember);
      }
    })
  }

  initializeUploader(){
    this.uploader = new FileUploader({
      url: this.baseUrl +'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10*1024*1024,
  });
  this.uploader.onAfterAddingFile = (file) => {
    file.withCredentials = false //making it false as we are passing the token in the header rather than in the cookies.
  }
  
  this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo = JSON.parse(response);
      const updateMember = {...this.member()}
      updateMember.photos.push(photo);
      this.memberChange.emit(updateMember);
      this.updateMainPhoto(photo);
  }
}

private updateMainPhoto(photo: Photo) {
  const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
        const updateMember = {...this.member()};
        updateMember.photoUrl = photo.url;
        updateMember.photos.forEach(p=>{
          if (p.isMain) p.isMain = false;
          if (p.id == photo.id) p.isMain = true;        
        })
        this.memberChange.emit(updateMember)
}
    
}
