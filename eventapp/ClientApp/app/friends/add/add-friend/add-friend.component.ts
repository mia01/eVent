import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import UserFriendRequest from 'ClientApp/app/models/UserFriendRequest';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FriendsService } from 'ClientApp/app/services/friends.service';

@Component({
  selector: 'app-add-friend',
  templateUrl: './add-friend.component.html',
  styleUrls: ['./add-friend.component.scss']
})
export class AddFriendComponent implements OnInit {
  userSearchForm: FormGroup;
  addUserForm: FormGroup;
  showNoUserFound: boolean = false;
  username: string = "";
  constructor(
    private friendsService: FriendsService,
    private formBuilder: FormBuilder,
    private modal: MatDialogRef<AddFriendComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any) { }

  ngOnInit(): void {
    this.userSearchForm = this.formBuilder.group({
      username: ["", Validators.required],
    });

    this.addUserForm = this.formBuilder.group({
      username: ["", Validators.required],
      userId: ["", Validators.required],
    });
  }

  submit(form) {
    if (form.valid) {
      this.modal.close(form.value as UserFriendRequest);
    }
  }
  
  async submitUserSearch(form) {
    if (form.valid) {
      // search for user and update userform
      var friend = await this.friendsService.findUserByUsername(form.value.username);
      if (friend != null) {
        this.addUserForm.controls['userId'].setValue(friend.userId);
        this.addUserForm.controls['username'].setValue(friend.username);
        this.showNoUserFound = false;
        this.username = friend.username;
      } else {
        this.addUserForm.controls['userId'].setValue("");
        this.addUserForm.controls['username'].setValue("");
        this.showNoUserFound = true;
        this.username = "";
      }
    }
  }
}
