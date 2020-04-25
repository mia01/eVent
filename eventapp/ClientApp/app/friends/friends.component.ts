import { Component, OnInit } from '@angular/core';
import UserFriend from '../models/UserFriend';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AddFriendComponent } from './add/add-friend/add-friend.component';
import { FriendsService } from '../services/friends.service';
import UserFriendRequest from '../models/UserFriendRequest';
import UserFriendResponse from '../models/UserFriendResponse';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent implements OnInit {
  addFriendModal: MatDialogRef<AddFriendComponent>;
  public friendRequests: UserFriendResponse[] = [];
  public acceptedFriendRequests: UserFriendResponse[] = [];
  public friendInvites: UserFriendResponse[] = [];

  constructor(private friendsService: FriendsService, private modal: MatDialog) { }

  async ngOnInit(): Promise<void> {
    this.friendRequests = await this.friendsService.getUserFriendRequests();
    this.friendInvites = await this.friendsService.getUserFriendInvites();
    this.acceptedFriendRequests = await this.friendsService.getUserFriends();
  }
  
  async openAddFriendModal() {
    this.addFriendModal = this.modal.open(AddFriendComponent, {
      disableClose: true,
      autoFocus : true
    });

    let formData = await this.addFriendModal.afterClosed().toPromise();
    await this.createFriendRequest(formData);
  }

  private async createFriendRequest(formData: any) {
    if (formData) {
      let newFriendResponse = await this.friendsService.createFriendRequest(formData as UserFriendRequest);

      if (newFriendResponse != null) {
        this.friendRequests.push(newFriendResponse);
      }
    }
  }

  public async acceptFriendInvite(friendInvite: UserFriendResponse) {
    let response = await this.friendsService.AcceptFriendRequest(friendInvite.id);

    if (response.accepted == true) {
      // update the currect data
      let index = this.friendInvites.indexOf(friendInvite);
      this.friendInvites.splice(index, 1);
      this.acceptedFriendRequests.push(response);
    }
  }
}
