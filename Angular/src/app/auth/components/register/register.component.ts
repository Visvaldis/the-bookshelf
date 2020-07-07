import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../login/login.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;




  constructor(private authService: AuthService,  private router: Router, private fb: FormBuilder) { }

  ngOnInit() {
    this.registerForm = this.fb.group(
      {
        userName: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
      },
      {validator: this.passwordMatchValidator}
    );
  }
  passwordMatchValidator(frm: FormGroup) {
    return frm.controls['password'].value === frm.controls['confirmPassword'].value ? null : {mismatch: true};
  }

  register() {
    this.authService.register(
      {
        email: this.registerForm.controls['userName'].value,
        password: this.registerForm.controls['password'].value,
        confirmPassword: this.registerForm.controls['confirmPassword'].value
      }).subscribe(regsuccess => {
      if (regsuccess) {
        this.authService.logIn(
          {
            userName: this.registerForm.controls['userName'].value,
            password: this.registerForm.controls['password'].value
          }).subscribe(success => {
          if (success) {
            this.router.navigate(['']);
          }
        });
      }
    });
  }

}
