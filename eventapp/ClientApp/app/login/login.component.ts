import { Component, OnInit } from '@angular/core';
import { Validators, FormControl, NgForm, FormBuilder, FormGroup } from '@angular/forms';
import { LoginService } from '../services/login.service';
import UserLogin from '../models/userLogin';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    invalidLogin: boolean
    loginForm: FormGroup;
    username = new FormControl('', [
        Validators.required,
        Validators.email,
    ]);
    password = new FormControl('', [
        Validators.required,
    ]);
    
    constructor(
        private loginService: LoginService, 
        private formBuilder: FormBuilder,
        private router: Router) 
    {
        this.loginForm = this.formBuilder.group({
            username: this.username, 
            password: this.password
        });
     }

    ngOnInit() {
    }

    async login(form) {
        if (form.valid) {
            var loggedIn = await this.loginService.login(form.value as UserLogin);

            if (loggedIn) {
                this.router.navigateByUrl("/app");
            } else {
                this.invalidLogin = true
            }
        }
    }

}
