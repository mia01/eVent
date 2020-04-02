import { Component, OnInit } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

    email = new FormControl('', [
        Validators.required,
        Validators.email,
    ]);
    password = new FormControl('', [
        Validators.required,
    ]);

    constructor() { }

    ngOnInit() {
    }

}
