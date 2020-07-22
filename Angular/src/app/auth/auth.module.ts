import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AppRoutingModule} from '../app-routing.module';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import {RouterModule} from '@angular/router';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {TokenInterceptor} from './token.interceptor';
import {AuthGuard} from './guards/auth.guard';
import {AuthService} from './services/auth.service';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';



@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [
    AuthGuard,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  exports: [LoginComponent, RegisterComponent],
})
export class AuthModule { }
