import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { catchError, mapTo, tap } from 'rxjs/operators';
import {config} from '../../../config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly TOKEN = 'TOKEN';
  private loggedUser: string;

  constructor(private http: HttpClient) {}

  login(user: { userName: string, password: string }): Observable<boolean> {
    return this.http.post<any>(`${config.apiUrl}/Token`, user)
      .pipe(
        tap(tokens => this.doLoginUser(user.userName, tokens)),
        mapTo(true),
        catchError(error => {
          alert(error.error);
          return of(false);
        }));
  }

  logIn(user: { userName: string, password: string }) {
    const url = `${config.apiUrl}/Token`;
    const body = `userName=${user.userName}&password=${user.password}&grant_type=password`;
    const heders = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
      'Access-Control-Allow-Headers': 'Content-Type',
      'Access-Control-Allow-Origin': '*'
    });

    this.http.post(url, body, {headers: heders},).subscribe(
      (data) => {
        console.log(data);
      },
      (err: HttpErrorResponse) => {
        if (err.error instanceof Error) {
          console.log('Client-side error occured.');
        } else {
          console.log('Server-side error occured.');
        }
      }
    );
  }


  logout() {
    this.doLogoutUser();
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  getToken() {
    return localStorage.getItem(this.TOKEN);
  }

  private doLoginUser(username: string, token: string) {
    this.loggedUser = username;
    this.storeTokens(token);
  }

  private doLogoutUser() {
    this.loggedUser = null;
    this.removeTokens();
  }

  private storeToken(jwt: string) {
    localStorage.setItem(this.TOKEN, jwt);
  }

  private storeTokens(token: string) {
    localStorage.setItem(this.TOKEN, token);
  }

  private removeTokens() {
    localStorage.removeItem(this.TOKEN);
  }
}
