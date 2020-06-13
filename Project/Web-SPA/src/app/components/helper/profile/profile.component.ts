import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/services/user.service';
import { DomSanitizer } from '@angular/platform-browser';

class ImageSnippet {
  pending = false;
  status = 'init';

  constructor(public src: string, public file: File) {}
}

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  userId: number;
  user: any;

  formOk = false;

  selectedFile: ImageSnippet;

  imageToShow: any;

  img;

  constructor(private route: ActivatedRoute, private san: DomSanitizer, private userService: UserService) {
    route.params.subscribe(params => {
      this.userId = params.id;
    });
  }

  private onSuccess() {
    this.selectedFile.pending = false;
    this.selectedFile.status = 'ok';
  }

  private onError() {
    this.selectedFile.pending = false;
    this.selectedFile.status = 'fail';
    this.selectedFile.src = '';
  }

  ngOnInit(): void {
    window.scroll(0, 0);
    const air1 = this.userService.getUser(this.userId).subscribe(
      (data: any) => {
        this.user = {
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email
        };
        this.img = this.san.bypassSecurityTrustResourceUrl(`data:image/png;base64, ${data.imageUrl}`);
        this.formOk = true;
      }
    );
    console.log(air1);
  }

  onFileChanged(imageInput: any) {
    const file: File = imageInput.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (event: any) => {

      this.selectedFile = new ImageSnippet(event.target.result, file);

      this.selectedFile.pending = true;
      const body = {
        image: this.selectedFile.file,
      };
      this.userService.changePhoto(body).subscribe(
        (res) => {
          this.onSuccess();
        },
        (err) => {
          this.onError();
        });
    });

    reader.readAsDataURL(file);
  }
}
