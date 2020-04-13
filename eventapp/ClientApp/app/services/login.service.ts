import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import UserLogin from '../models/userLogin';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private LOGIN_URL = environment.api.base + environment.api.login;
  private loggedIn: boolean;
  constructor(
    private httpClient: HttpClient
  ) { }


  public async login(userLogin: UserLogin): Promise<boolean> {
    try {
      var response = await this.httpClient.post(this.LOGIN_URL, userLogin).toPromise();
      let token = (<any> response).token;
      localStorage.setItem("jwt", token);
      this.loggedIn = true;
      return this.loggedIn;

    } catch (error) {
      this.loggedIn = false;
      return this.loggedIn;
    }
  }

  public logout(): void {
    localStorage.removeItem("jwt");
  }
}
