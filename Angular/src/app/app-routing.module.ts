import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {TagsComponent} from './components/tags/tags.component';
import {LoginComponent} from './auth/components/login/login.component';
import {RegisterComponent} from './auth/components/register/register.component';

import {AuthGuard} from './auth/guards/auth.guard';
import {ProfileComponent} from './auth/components/profile/profile.component';
import {ProfileGuard} from './auth/guards/profile.guard';

// определение маршрутов
const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'login', component: LoginComponent, canActivate : [AuthGuard]},
  { path: 'register', component: RegisterComponent, canActivate : [AuthGuard]},
  { path: 'profile', component: ProfileComponent, canActivate : [ProfileGuard]},
  { path: 'tags', component: TagsComponent},
  { path: '**', redirectTo: ''}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
