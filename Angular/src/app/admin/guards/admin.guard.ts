import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AuthService} from '../../auth/services/auth.service';
import {AuthUser} from '../../auth/models/auth.user';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate() {
    const user: AuthUser = this.authService.getUser();
    if (!user.role.includes('admin')) {
      this.router.navigate(['/']);
    }
    return this.authService.isLoggedIn();
  }
}
