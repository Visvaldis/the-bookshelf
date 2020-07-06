import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {config} from '../../../config';
import {AuthUser} from '../models/auth.user';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly TOKEN = 'TOKEN';
  private loggedUser: AuthUser;

  constructor(private http: HttpClient) {}

  logIn(user: { userName: string, password: string }) {
    const url = `${config.apiUrl}/Token`;
    const body = new URLSearchParams();
    body.set('userName', user.userName);
    body.set('password', user.password);
    body.set('grant_type', 'password');

    const authheaders = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
    });

    this.http.post(url, body.toString(), {headers: authheaders}).subscribe(
      (data) => {
      //  console.log(data);
        this.doLoginUser(data);
      },
      (err: HttpErrorResponse) => {
        if (err.error instanceof Error) {
          console.log('Client-side error occured.');
          console.log(err.message);
          console.log(err.error);
        } else {
          console.log('Server-side error occured.');
          console.log(err.message);
          console.log(err.error);
        }
      }
    );
  }


  logout() {
    this.doLogoutUser();
  }

  isLoggedIn(): boolean {
    const b: boolean = !!this.getToken();
    return b;
  }

  getToken() {
    return localStorage.getItem(this.TOKEN);
  }
  getUser() {
    return this.loggedUser;
  }
  private doLoginUser(data) {
    const token = data.access_token;
    this.loggedUser =
    {
      name: data.userName,
      role: data.userRole,
    };
    console.log(this.loggedUser);
    this.storeToken(token);
  }

  private doLogoutUser() {
    this.loggedUser = null;
    this.removeTokens();
  }


  private storeToken(token: string) {
    localStorage.setItem(this.TOKEN, token);
  }

  private removeTokens() {
    localStorage.removeItem(this.TOKEN);
  }
}
