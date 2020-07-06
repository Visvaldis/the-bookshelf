import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({

    userName: new FormControl('', [
      Validators.required,
      Validators.email
    ]),
    password: new FormControl('')
  });

  constructor(private authService: AuthService,  private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.logIn(
      {
        userName: this.loginForm.controls['userName'].value,
        password: this.loginForm.controls['password'].value
      }
    );
  }

}
