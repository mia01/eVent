import { CanActivate, Router } from '@angular/router';
import { getJwtToken } from 'ClientApp/app/models/auth/auth.utils';
import { Injectable } from '@angular/core';
import { JwtHelperService  } from "@auth0/angular-jwt";
import { LoginService } from './login.service';

@Injectable({
    providedIn: 'root'
})

export class AuthGuard implements CanActivate {
    constructor(
        private loginService: LoginService,
        private router: Router) {}
    
    canActivate(): boolean {
        if (this.loginService.isUserAuthenticated()) {
            return true;
        }

        this.router.navigateByUrl("/login");
        return false;
    }
}