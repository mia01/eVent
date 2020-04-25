import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import UserFriendRequest from '../models/UserFriendRequest';
import UserFriendResponse from '../models/UserFriendResponse';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  private FRIEND_URL = environment.api.base + environment.api.friends;

  constructor(
    private httpClient: HttpClient
  ) { }

  public findUserByUsername(username: string): Promise<UserFriendRequest> {
    return this.httpClient.get<UserFriendRequest>(this.FRIEND_URL + "/FindByUsername", {
      params: {username: username}
    })
    .toPromise();
  }

  public getUserFriends(): Promise<UserFriendResponse[]> {
    return this.httpClient.get<UserFriendResponse[]>(this.FRIEND_URL + "/GetUserFriends")
    .toPromise();
  }
  
  public getUserFriendRequests(): Promise<UserFriendResponse[]> {
    return this.httpClient.get<UserFriendResponse[]>(this.FRIEND_URL + "/GetUserFriendRequests")
    .toPromise();
  }

  public getUserFriendInvites(): Promise<UserFriendResponse[]> {
    return this.httpClient.get<UserFriendResponse[]>(this.FRIEND_URL + "/GetUserFriendInvites")
    .toPromise();
  }
  
  public createFriendRequest(friendRequest: UserFriendRequest): Promise<UserFriendResponse> {
    return this.httpClient.post<UserFriendResponse>(this.FRIEND_URL + "/CreateFriendRequest", friendRequest)
    .toPromise();
  }

  public AcceptFriendRequest(requestId: number): Promise<UserFriendResponse> {
    return this.httpClient.post<UserFriendResponse>(this.FRIEND_URL + "/AcceptFriendRequest", requestId)
    .toPromise();
  }
}
