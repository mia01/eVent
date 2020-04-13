import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import UserLogin from '../../models/userLogin';
import { getJwtToken, setJwtToken, removeJwtToken } from 'ClientApp/app/models/auth/auth.utils';
import { JwtHelperService  } from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private LOGIN_URL = environment.api.base + environment.api.login;
  private loggedIn: boolean;
  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) { }


  public async login(userLogin: UserLogin): Promise<boolean> {
    try {
      var response = await this.httpClient.post(this.LOGIN_URL, userLogin).toPromise();
      let token = (<any> response).token;
      setJwtToken(token);
      this.loggedIn = true;
      return this.loggedIn;

    } catch (error) {
      this.loggedIn = false;
      return this.loggedIn;
    }
  }

  public logout(): void {
    removeJwtToken();
  }

  isUserAuthenticated() {
    let token: string = getJwtToken();
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    else {
      return false;
    }
  }
}
